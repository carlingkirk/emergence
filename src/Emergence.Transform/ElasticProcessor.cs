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
    public class ElasticProcessor
    {
        private readonly IRepository<PlantInfo> _plantInfoRepository;
        private readonly IIndex<SearchModels.PlantInfo> _plantInfoIndex;
        public ElasticProcessor(IRepository<PlantInfo> plantInfoRepository, IIndex<SearchModels.PlantInfo> plantInfoIndex)
        {
            _plantInfoRepository = plantInfoRepository;
            _plantInfoIndex = plantInfoIndex;
        }

        public async Task Process(int startId, int endId)
        {
            var plantInfoQuery = _plantInfoRepository.WhereWithIncludes(p => p.Id >= startId && p.Id <= endId, false,
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
                                                                              .Include(p => p.MinimumZone)
                                                                              .Include(p => p.MaximumZone));

            var plantInfoResult = await plantInfoQuery.GetAllAsync();

            var plantInfos = new List<SearchModels.PlantInfo>();
            foreach (var plantInfo in plantInfoResult.GroupBy(p => p.Id))
            {
                var plantInfoKey = plantInfo.First();
                var plantLocations = plantInfo.SelectMany(p => p.PlantLocations);
                var synonyms = plantInfoKey.Taxon != null ? plantInfo.SelectMany(p => p.Taxon?.Synonyms) : null;
                plantInfos.Add(plantInfoKey.AsSearchModel(plantLocations, synonyms));
            }

            await _plantInfoIndex.IndexManyAsync(plantInfos);
        }
    }
}
