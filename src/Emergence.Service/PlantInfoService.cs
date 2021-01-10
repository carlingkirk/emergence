using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            var plantInfoSearch = await _plantInfoIndex.SearchAsync(findParams.SearchText);
            var plantInfoIds = plantInfoSearch.Documents.Select(p => p.Id);
            var plantInfoQuery = _plantInfoRepository.WhereWithIncludes(p => plantInfoIds.Contains(p.Id),
                                                                        false,
                                                                        p => p.Include(p => p.Lifeform)
                                                                              .Include(p => p.Taxon)
                                                                              .Include(p => p.Origin)
                                                                              .Include(p => p.User)
                                                                              .Include(p => p.MinimumZone).Include(p => p.MaximumZone));

            //var plantInfoQuery = _plantInfoRepository.WhereWithIncludes(p => (findParams.SearchTextQuery == null ||
            //                                                            EF.Functions.Like(p.CommonName, findParams.SearchTextQuery) ||
            //                                                            EF.Functions.Like(p.ScientificName, findParams.SearchTextQuery) ||
            //                                                            EF.Functions.Like(p.Lifeform.CommonName, findParams.SearchTextQuery) ||
            //                                                            EF.Functions.Like(p.Lifeform.ScientificName, findParams.SearchTextQuery)),
            //                                                            false,
            //                                                            p => p.Include(p => p.Lifeform)
            //                                                                  .Include(p => p.Taxon)
            //                                                                  .Include(p => p.Origin)
            //                                                                  .Include(p => p.User)
            //                                                                  .Include(p => p.MinimumZone).Include(p => p.MaximumZone));
            //if (!string.IsNullOrEmpty(findParams.CreatedBy))
            //{
            //    plantInfoQuery = plantInfoQuery.Where(p => p.CreatedBy == findParams.CreatedBy);
            //}

            //plantInfoQuery = FilterBy(plantInfoQuery, findParams.Filters);

            //plantInfoQuery = plantInfoQuery.CanViewContent(user);

            //plantInfoQuery = OrderBy(plantInfoQuery, findParams.SortBy, findParams.SortDirection);

            //var count = plantInfoQuery.Count();

            var plantInfoResult = plantInfoQuery.GetSomeAsync(skip: findParams.Skip, take: findParams.Take, track: false);

            var plantInfos = new List<Data.Shared.Models.PlantInfo>();
            await foreach (var plantInfo in plantInfoResult)
            {
                plantInfos.Add(plantInfo.AsModel());
            }

            return new FindResult<Data.Shared.Models.PlantInfo>
            {
                Count = plantInfoSearch.Count,
                Results = plantInfos
            };
        }

        private IQueryable<PlantInfo> FilterBy(IQueryable<PlantInfo> plantInfoQuery, IEnumerable<Filter> filters)
        {
            if (filters == null)
            {
                return plantInfoQuery;
            }

            foreach (var filter in filters)
            {
                if (filter.Name == "Location")
                {
                    var regionFilter = new RegionFilter((Filter<string>)filter);
                    if (!string.IsNullOrEmpty(regionFilter.Value))
                    {
                        plantInfoQuery = plantInfoQuery.Where(regionFilter.Filter);
                    }
                }
                else if (filter.Name == "Height")
                {
                    var heightFilter = new HeightFilter((RangeFilter<double?>)filter);
                    if (heightFilter.MinimumValue.HasValue|| heightFilter.MaximumValue.HasValue)
                    {
                        plantInfoQuery = plantInfoQuery.Where(heightFilter.Filter);
                    }
                }
                else if (filter.Name == "Spread")
                {
                    var spreadFilter = new SpreadFilter((RangeFilter<double?>)filter);
                    if (spreadFilter.MinimumValue.HasValue|| spreadFilter.MaximumValue.HasValue)
                    {
                        plantInfoQuery = plantInfoQuery.Where(spreadFilter.Filter);
                    }
                }
                else if (filter.Name == "Light")
                {
                    var lightFilter = new LightFilter((RangeFilter<string>)filter);
                    if (!(string.IsNullOrEmpty(lightFilter.MinimumValue) && string.IsNullOrEmpty(lightFilter.MaximumValue)))
                    {
                        plantInfoQuery = plantInfoQuery.Where(lightFilter.Filter);
                    }
                }
                else if (filter.Name == "Water")
                {
                    var waterFilter = new WaterFilter((RangeFilter<string>)filter);
                    if (!(string.IsNullOrEmpty(waterFilter.MinimumValue) && string.IsNullOrEmpty(waterFilter.MaximumValue)))
                    {
                        plantInfoQuery = plantInfoQuery.Where(waterFilter.Filter);
                    }
                }
                else if (filter.Name == "Bloom")
                {
                    var bloomFilter = new BloomFilter((RangeFilter<int>)filter);
                    if (!(bloomFilter.MinimumValue == 0 && bloomFilter.MaximumValue == 0))
                    {
                        plantInfoQuery = plantInfoQuery.Where(bloomFilter.Filter);
                    }
                }
                else if (filter.Name == "Zone")
                {
                    var zoneFilter = new ZoneFilter((SelectFilter<int>)filter);
                    if (zoneFilter.Value > 0)
                    {
                        plantInfoQuery = plantInfoQuery.Where(zoneFilter.Filter);
                    }
                }
            }

            return plantInfoQuery;
        }

        private IQueryable<PlantInfo> OrderBy(IQueryable<PlantInfo> plantInfoQuery, string sortBy = null, SortDirection sortDirection = SortDirection.None)
        {
            if (sortDirection == SortDirection.None)
            {
                return plantInfoQuery;
            }

            if (sortBy == null)
            {
                sortBy = "DateCreated";
            }

            var plantInfoSorts = new Dictionary<string, Expression<Func<PlantInfo, object>>>
            {
                { "ScientificName", p => p.Lifeform.ScientificName },
                { "CommonName", p => p.Lifeform.CommonName },
                { "Origin", p => p.Origin.Name },
                { "Zone", p => p.MinimumZone },
                { "Light", p => p.MinimumLight },
                { "Water", p => p.MinimumWater },
                { "BloomTime", p => p.MinimumBloomTime },
                { "Height", p => p.MinimumHeight },
                { "Spread", p => p.MinimumSpread },
                { "DateCreated", p => p.DateCreated }
            };

            if (sortDirection == SortDirection.Descending)
            {
                plantInfoQuery = plantInfoQuery.WithOrder(p => p.OrderByDescending(plantInfoSorts[sortBy]));
            }
            else
            {
                plantInfoQuery = plantInfoQuery.WithOrder(p => p.OrderBy(plantInfoSorts[sortBy]));
            }

            return plantInfoQuery;
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
