using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Emergence.Data.Shared.Search.Models;
using Microsoft.Extensions.Configuration;
using Nest;

namespace Emergence.Service.Search
{
    public class SearchClient<T> : ISearchClient<T> where T : class
    {
        private ElasticClient ElasticClient;
        private readonly ConnectionSettings _connectionSettings;

        public SearchClient(IConfiguration configuration)
        {
            var cloudId = configuration["ElasticCloudId"];
            var username = configuration["ElasticUsername"];
            var password = configuration["ElasticPassword"];
            var basicCredentials = new BasicAuthenticationCredentials(username, password);

            _connectionSettings = new ConnectionSettings(cloudId, basicCredentials);
        }

        public async Task ConfigureClient(string indexName, Func<ClrTypeMappingDescriptor<T>, IClrTypeMapping<T>> selector)
        {
            _connectionSettings.DefaultMappingFor<T>(selector);

            ElasticClient = new ElasticClient(_connectionSettings);
            var auth = await ElasticClient.Security.AuthenticateAsync();
            if (!auth.IsValid)
            {
                throw new UnauthorizedAccessException();
            }
            await CreateIndexAsync(indexName);
        }

        public async Task<IEnumerable<PlantInfo>> SearchAsync(string search)
        {
            var response = await ElasticClient.SearchAsync<PlantInfo>(s => s
              .Query(q => q.Bool(qc => qc
                .Should(q => q
                  .Match(c => c
                    .Field(m => m.CommonName)
                    .Field(m => m.ScientificName)
                    .Field(m => m.Lifeform.CommonName)
                    .Field(m => m.Lifeform.ScientificName)
                    .Query(search)
                    .Fuzziness(Fuzziness.AutoLength(1, 5))))
                .Should(q => q.Nested(qc => qc.Path("plantSynyonyms")
                    .Query(q => q.Term("plantSynyonyms.synonym", search)))))));

            return response.Documents;
        }

        public async Task CreateIndexAsync(string indexName)
        {
            var indexExists = await IndexExistsAsync(indexName);
            if (!indexExists)
            {
                var createIndexResponse = await ElasticClient.Indices.CreateAsync(indexName);

                if (!createIndexResponse.ApiCall.Success)
                {
                    throw createIndexResponse.ApiCall.OriginalException;
                }
            }
        }

        public async Task<bool> IndexAsync(T document)
        {
            var response = await ElasticClient.IndexDocumentAsync(document);
            var result = true;

            if (!response.IsValid)
            {
                result = false;
            }

            return result;
        }

        public async Task<BulkIndexResponse> IndexManyAsync(IEnumerable<T> documents)
        {
            var response = new BulkIndexResponse();
            var indexManyResponse = await ElasticClient.IndexManyAsync(documents);
            response.Successes = indexManyResponse.Items.Count(i => i.IsValid);
            response.Failures = indexManyResponse.Items.Count(i => !i.IsValid);

            if (indexManyResponse.Errors)
            {
                response.Errors = indexManyResponse.ItemsWithErrors.Select(i => $"{i.Operation}: {i.Status} - {i.Result} for {i.Index}");
            }

            return response;
        }

        private async Task<bool> IndexExistsAsync(string name)
        {
            var aliasResponse = await ElasticClient.Indices.ExistsAsync(name);

            return aliasResponse.Exists;
        }
    }

    public class BulkIndexResponse
    {
        public int Successes { get; set; }
        public int Failures { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
