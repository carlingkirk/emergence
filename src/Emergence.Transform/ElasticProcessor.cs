using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Extensions;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;

namespace Emergence.Transform
{
    public class ElasticProcessor
    {
        private readonly IRepository<PlantInfo> _plantInfoRepository;
        private readonly IRepository<PlantLocation> _plantLocationRepository;
        public ElasticProcessor(IRepository<PlantInfo> plantInfoRepository, IRepository<PlantLocation> plantLocationRepository)
        {
            _plantInfoRepository = plantInfoRepository;
            _plantLocationRepository = plantLocationRepository;
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

            var plantInfos = new List<Emergence.Data.Shared.Search.Models.PlantInfo>();
            foreach (var plantInfo in plantInfoResult.GroupBy(p => p.Id))
            {
                var plantLocations = plantInfo.SelectMany(p => p.PlantLocations);
                var synonyms = plantInfo.SelectMany(p => p.Taxon.Synonyms);
                plantInfos.Add(plantInfo.First().AsSearchModel(plantLocations, synonyms));
            }
        }
    }
}
