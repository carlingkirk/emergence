using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Extensions;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Search;
using Emergence.Data.Shared.Stores;
using Emergence.Service.Interfaces;
using Emergence.Service.Search;
using Microsoft.EntityFrameworkCore;
using SearchModels = Emergence.Data.Shared.Search.Models;

namespace Emergence.Service
{
    public class PlantInfoService : IPlantInfoService
    {
        private readonly IRepository<PlantInfo> _plantInfoRepository;
        private readonly IRepository<PlantLocation> _plantLocationRepository;
        private readonly IIndex<SearchModels.PlantInfo> _plantInfoIndex;

        public PlantInfoService(IRepository<PlantInfo> plantInfoRepository, IRepository<PlantLocation> plantLocationRepository, IIndex<SearchModels.PlantInfo> plantInfoIndex)
        {
            _plantInfoRepository = plantInfoRepository;
            _plantLocationRepository = plantLocationRepository;
            _plantInfoIndex = plantInfoIndex;
        }

        public async Task<Data.Shared.Models.PlantInfo> AddOrUpdatePlantInfoAsync(Data.Shared.Models.PlantInfo plantInfo)
        {
            plantInfo.DateModified = DateTime.UtcNow;
            var plantInfoResult = await _plantInfoRepository.AddOrUpdateAsync(l => l.Id == plantInfo.PlantInfoId, plantInfo.AsStore());
            return plantInfoResult.AsModel();
        }

        public async Task<Data.Shared.Models.PlantInfo> GetPlantInfoAsync(int id, Data.Shared.Models.User user)
        {
            var plantInfoQuery = _plantInfoRepository.WhereWithIncludes(p => p.Id == id, false,
                                                                        p => p.Include(p => p.Lifeform)
                                                                              .Include(p => p.Origin)
                                                                              .Include(p => p.User)
                                                                              .Include(p => p.User.Photo)
                                                                              .Include(p => p.MinimumZone).Include(p => p.MaximumZone));
            plantInfoQuery = plantInfoQuery.CanViewContent(user);

            var plantInfo = await plantInfoQuery.FirstOrDefaultAsync();
            return plantInfo?.AsModel();
        }

        public async Task<Data.Shared.Models.PlantInfo> GetPlantInfoAsync(int originId, string scientificName)
        {
            var plantInfo = await _plantInfoRepository.GetAsync(p => p.OriginId == originId && p.ScientificName == scientificName);
            return plantInfo?.AsModel();
        }

        public async Task<Data.Shared.Models.PlantInfo> GetPlantInfoAsync(int originId, int taxonId)
        {
            var plantInfo = await _plantInfoRepository.GetAsync(p => p.OriginId == originId && p.TaxonId == taxonId);
            return plantInfo?.AsModel();
        }

        public async Task<FindResult<Data.Shared.Models.PlantInfo>> FindPlantInfos(FindParams findParams, Data.Shared.Models.User user)
        {
            var plantInfoSearch = await _plantInfoIndex.SearchAsync(findParams, user);
            var plantInfoIds = plantInfoSearch.Documents.Select(p => p.Id).ToArray();
            var plantInfoQuery = _plantInfoRepository.WhereWithIncludes(p => plantInfoIds.Contains(p.Id),
                                                                        false,
                                                                        p => p.Include(p => p.Lifeform)
                                                                              .Include(p => p.Taxon)
                                                                              .Include(p => p.Origin)
                                                                              .Include(p => p.User)
                                                                              .Include(p => p.MinimumZone).Include(p => p.MaximumZone));

            plantInfoQuery = plantInfoQuery.CanViewContent(user);

            var plantInfoResult = plantInfoQuery.GetSomeAsync(track: false);

            var plantInfos = new List<Data.Shared.Models.PlantInfo>();
            await foreach (var plantInfo in plantInfoResult)
            {
                plantInfos.Add(plantInfo.AsModel());
            }

            var filters = FilterList.GetPlantInfoFilters();
            foreach (var aggregation in plantInfoSearch.Aggregations)
            {
                var filter = filters.Where(f => f.Name == aggregation.Name).First() as SelectFilter<string>;
                filter.FacetValues = aggregation.Values;
            }

            return new FindResult<Data.Shared.Models.PlantInfo>
            {
                Count = plantInfoSearch.Count,
                Results = plantInfoIds.Join(plantInfos, pid => pid, pi => pi.PlantInfoId, (id, p) => p).ToList(),
                Filters = filters
            };
        }

        public async Task<IEnumerable<Data.Shared.Models.PlantInfo>> AddPlantInfosAsync(IEnumerable<Data.Shared.Models.PlantInfo> plantInfos)
        {
            var plantInfosResult = await _plantInfoRepository.AddSomeAsync(plantInfos.Select(o => o.AsStore()));
            return plantInfosResult.Select(o => o.AsModel());
        }

        public async Task<IEnumerable<Data.Shared.Models.PlantLocation>> AddPlantLocations(IEnumerable<Data.Shared.Models.PlantLocation> plantLocations)
        {
            var plantLocationsResult = await _plantLocationRepository.AddSomeAsync(plantLocations.Select(o => o.AsStore()));
            return plantLocationsResult.Select(o => o.AsModel());
        }

        public async Task<bool> RemovePlantInfoAsync(Data.Shared.Models.PlantInfo plantInfo) => await _plantInfoRepository.RemoveAsync(plantInfo.AsStore());
    }
}
