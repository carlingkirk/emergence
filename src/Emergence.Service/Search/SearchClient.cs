using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elasticsearch.Net;
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
            _connectionSettings.DisableDirectStreaming().PrettyJson();
        }

        public async Task ConfigureClient(string indexName, Func<ClrTypeMappingDescriptor<T>, IClrTypeMapping<T>> selector, Func<TypeMappingDescriptor<T>, ITypeMapping> mappingSelector)
        {
            _connectionSettings.DefaultMappingFor<T>(selector);

            ElasticClient = new ElasticClient(_connectionSettings);
            var auth = await ElasticClient.Security.AuthenticateAsync();
            if (!auth.IsValid)
            {
                throw new UnauthorizedAccessException();
            }

            await CreateIndexAsync(indexName, mappingSelector);
        }

        public async Task<SearchResponse<T>> SearchAsync(Func<QueryContainerDescriptor<T>, QueryContainer> query, int skip, int take)
        {
            var response = await ElasticClient.SearchAsync<T>(s => s
              .Query(query)
              .Take(take)
              .Skip(skip));

            var body = Encoding.UTF8.GetString(response.ApiCall.RequestBodyInBytes);
            Console.WriteLine(body);

            return new SearchResponse<T>
            {
                Documents = response.Documents,
                Count = response.Hits.Count
            };
        }

        private async Task CreateIndexAsync(string indexName, Func<TypeMappingDescriptor<T>, ITypeMapping> mappingSelector)
        {
            var indexExists = await IndexExistsAsync(indexName);
            if (!indexExists)
            {
                var createIndexResponse = await ElasticClient.Indices.CreateAsync(indexName, i => i.Map(mappingSelector));

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

    public class SearchResponse<T>
    {
        public int Count { get; set; }
        public IEnumerable<T> Documents { get; set; }
    }
}
