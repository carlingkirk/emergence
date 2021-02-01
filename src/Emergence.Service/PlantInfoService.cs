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
        private readonly IIndex<SearchModels.PlantInfo, Data.Shared.Models.PlantInfo> _plantInfoIndex;

        public PlantInfoService(IRepository<PlantInfo> plantInfoRepository, IRepository<PlantLocation> plantLocationRepository, IIndex<SearchModels.PlantInfo, Data.Shared.Models.PlantInfo> plantInfoIndex)
        {
            _plantInfoRepository = plantInfoRepository;
            _plantLocationRepository = plantLocationRepository;
            _plantInfoIndex = plantInfoIndex;
        }

        public async Task<Data.Shared.Models.PlantInfo> AddOrUpdatePlantInfoAsync(Data.Shared.Models.PlantInfo plantInfo)
        {
            plantInfo.DateModified = DateTime.UtcNow;
            var plantInfoResult = await _plantInfoRepository.AddOrUpdateAsync(l => l.Id == plantInfo.PlantInfoId, plantInfo.AsStore());
            if (plantInfoResult != null)
            {
                var plantInfoLifeform = await _plantInfoRepository.GetWithIncludesAsync(p => p.Id == plantInfoResult.Id, false,
                                                                                  p => p.Include(p => p.Lifeform)
                                                                                        .Include(p => p.Origin)
                                                                                        .Include(p => p.User)
                                                                                        .Include(p => p.User.Photo)
                                                                                        .Include(p => p.MinimumZone).Include(p => p.MaximumZone));
                await _plantInfoIndex.IndexAsync(plantInfoLifeform.AsSearchModel(null, null));
            }
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

        public async Task<PlantInfoFindResult> FindPlantInfos(PlantInfoFindParams findParams, Data.Shared.Models.User user)
        {
            if (findParams.Filters == null)
            {
                findParams.Filters = new PlantInfoFilters();
            }

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

            if (plantInfoSearch.Aggregations != null)
            {
                foreach (var aggregation in plantInfoSearch.AggregationResult)
                {
                    var filter = PlantInfoFindParams.GetFilter(aggregation.Name, findParams);

                    if (filter is SelectFilter<string> selectFilter)
                    {
                        var values = aggregation.Values;
                        values = values.Prepend(new KeyValuePair<string, long?>("", null)).ToDictionary(k => k.Key, v => v.Value);
                        selectFilter.FacetValues = values;
                    }
                    if (filter is SelectRangeFilter<double> selectRangeFilter)
                    {
                        var values = aggregation.Values.ToDictionary(k => double.Parse(k.Key), v => v.Value).OrderBy(k => k.Key).ToDictionary(k => k.Key, v => v.Value);
                        if (aggregation.Name.Contains("Min"))
                        {
                            selectRangeFilter.MinFacetValues = values;
                        }
                        else
                        {
                            selectRangeFilter.MaxFacetValues = values;
                        }
                    }
                    if (filter is RangeFilter<string> rangeFilter)
                    {
                        var values = aggregation.Values;
                        if (aggregation.Name == "Bloom")
                        {
                            values = aggregation.Values.ToDictionary(k => int.Parse(k.Key), v => v.Value).OrderBy(k => k.Key).ToDictionary(k => k.Key.ToString(), v => v.Value);
                        }

                        values = values.Prepend(new KeyValuePair<string, long?>("", null)).ToDictionary(k => k.Key, v => v.Value);

                        rangeFilter.FacetValues = values;
                    }
                }
            }

            return new PlantInfoFindResult
            {
                Count = plantInfoSearch.Count,
                Results = plantInfoIds.Join(plantInfos, pid => pid, pi => pi.PlantInfoId, (id, p) => p).ToList(),
                Filters = findParams.Filters
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
