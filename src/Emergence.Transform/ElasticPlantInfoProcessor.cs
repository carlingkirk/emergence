using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Extensions;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;
using Emergence.Service.Search;
using SearchModels = Emergence.Data.Shared.Search.Models;

namespace Emergence.Transform
{
    public class ElasticPlantInfoProcessor : IElasticProcessor<PlantInfo>
    {
        private readonly IRepository<PlantInfo> _plantInfoRepository;
        private readonly IIndex<SearchModels.PlantInfo, Emergence.Data.Shared.Models.PlantInfo> _plantInfoIndex;

        public ElasticPlantInfoProcessor(IRepository<PlantInfo> plantInfoRepository, IIndex<SearchModels.PlantInfo, Emergence.Data.Shared.Models.PlantInfo> plantInfoIndex)
        {
            _plantInfoRepository = plantInfoRepository;
            _plantInfoIndex = plantInfoIndex;
        }

        public async Task<BulkIndexResponse> Process(int startId, int endId)
        {
            var plantInfoQuery = _plantInfoRepository.WhereWithIncludes(p => p.Id >= startId && p.Id < endId, false,
                                                                        p => p.Include(p => p.Lifeform)
                                                                              .Include(p => p.Lifeform.PlantSynonyms)
                                                                              .Include(p => p.PlantLocations)
                                                                                .ThenInclude(pl => pl.Location)
                                                                              .Include(p => p.Taxon)
                                                                              .Include(p => p.Taxon.Synonyms)
                                                                              .Include(p => p.Origin)
                                                                              .Include(p => p.Origin.Location)
                                                                              .Include(p => p.User)
                                                                              .Include(p => p.User.Photo)
                                                                              .Include(p => p.User.Contacts)
                                                                              .Include(p => p.MinimumZone)
                                                                              .Include(p => p.MaximumZone));

            var plantInfoResult = await plantInfoQuery.GetAllAsync();
            if (plantInfoResult.Count() == 0)
            {
                return new BulkIndexResponse { Successes = 0, Failures = 0 };
            }

            var plantInfos = new List<SearchModels.PlantInfo>();
            foreach (var plantInfo in plantInfoResult.GroupBy(p => p.Id))
            {
                var plantInfoKey = plantInfo.First();
                var plantLocations = plantInfo.SelectMany(p => p.PlantLocations).ToList();
                var synonyms = plantInfoKey.Taxon != null ? plantInfo.SelectMany(p => p.Taxon?.Synonyms).ToList() : null;
                if (plantInfoKey.User != null && plantInfoKey.User.Contacts != null)
                {
                    var contacts = plantInfo.SelectMany(p => p.User.Contacts).ToList();
                    plantInfoKey.User.Contacts = contacts;
                }

                plantInfos.Add(plantInfoKey.AsSearchModel(plantLocations, synonyms));
            }

            return await _plantInfoIndex.IndexManyAsync(plantInfos);
        }
    }
}
