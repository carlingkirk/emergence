using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared.Search.Models;
using Microsoft.Extensions.Configuration;
using Nest;

namespace Emergence.Service.Search
{
    public class SearchClient
    {
        private ElasticClient Client { get; set; }

        public SearchClient(IConfiguration configuration)
        {
            var node = new Uri(configuration["ElasticEndpoint"]);
            var settings = new ConnectionSettings(node);
            Client = new ElasticClient(settings);
        }

        public async Task<IEnumerable<PlantInfo>> SearchAsync(string search)
        {
            var response = await Client.SearchAsync<PlantInfo>(s => s
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

        public async Task<bool> SaveAsync(PlantInfo plantInfo)
        {
            var response = await Client.IndexDocumentAsync(plantInfo);
            var result = true;

            if (!response.IsValid)
            {
                result = false;
            }

            return result;
        }

        public async Task<bool> SaveManyAsync(IEnumerable<PlantInfo> plantInfos)
        {
            var indexManyResponse = await Client.IndexManyAsync(plantInfos);
            var result = true;

            if (indexManyResponse.Errors)
            {
                foreach (var item in indexManyResponse.ItemsWithErrors)
                {
                    //log
                }

                result = false;
            }
            return result;
        }
    }
}
