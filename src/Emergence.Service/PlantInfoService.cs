using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Extensions;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;
using Emergence.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Emergence.Service
{
    public class PlantInfoService : IPlantInfoService
    {
        private readonly IRepository<PlantInfo> _plantInfoRepository;
        public PlantInfoService(IRepository<PlantInfo> plantInfoRepository)
        {
            _plantInfoRepository = plantInfoRepository;
        }

        public async Task<Data.Shared.Models.PlantInfo> AddOrUpdatePlantInfoAsync(Data.Shared.Models.PlantInfo plantInfo)
        {
            plantInfo.DateModified = DateTime.UtcNow;
            var plantInfoResult = await _plantInfoRepository.AddOrUpdateAsync(l => l.Id == plantInfo.PlantInfoId, plantInfo.AsStore());
            return plantInfoResult.AsModel();
        }

        public async Task<Data.Shared.Models.PlantInfo> GetPlantInfoAsync(int id)
        {
            var plantInfo = await _plantInfoRepository.GetWithIncludesAsync(l => l.Id == id, false, p => p.Include(p => p.Lifeform).Include(p => p.Origin));
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

        public async Task<IEnumerable<Data.Shared.Models.PlantInfo>> GetPlantInfosAsync()
        {
            var plantInfoResult = _plantInfoRepository.GetSomeAsync(l => l.Id > 0);
            var plantInfos = new List<Data.Shared.Models.PlantInfo>();
            await foreach (var plantInfo in plantInfoResult)
            {
                plantInfos.Add(plantInfo.AsModel());
            }
            return plantInfos;
        }

        public async Task<FindResult<Data.Shared.Models.PlantInfo>> FindPlantInfos(FindParams findParams)
        {
            if (findParams.SearchText != null)
            {
                findParams.SearchText = "%" + findParams.SearchText + "%";
            }

            var plantInfoQuery = _plantInfoRepository.WhereWithIncludesAsync(p => (findParams.SearchText == null ||
                                                                       EF.Functions.Like(p.CommonName, findParams.SearchText) ||
                                                                       EF.Functions.Like(p.ScientificName, findParams.SearchText) ||
                                                                        EF.Functions.Like(p.Lifeform.CommonName, findParams.SearchText) ||
                                                                        EF.Functions.Like(p.Lifeform.ScientificName, findParams.SearchText)),
                                                                        p => p.Include(p => p.Lifeform)
                                                                              .Include(p => p.Taxon)
                                                                              .Include(p => p.Origin));

            plantInfoQuery = OrderBy(plantInfoQuery, findParams.SortBy, findParams.SortDirection);

            var count = plantInfoQuery.Count();

            var plantInfoResult = plantInfoQuery.GetSomeAsync(skip: findParams.Skip, take: findParams.Take, track: false);

            var plantInfos = new List<Data.Shared.Models.PlantInfo>();
            await foreach (var plantInfo in plantInfoResult)
            {
                plantInfos.Add(plantInfo.AsModel());
            }

            return new FindResult<Data.Shared.Models.PlantInfo>
            {
                Count = count,
                Results = plantInfos
            };
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
                { "ScientificName", p => p.ScientificName },
                { "CommonName", p => p.CommonName },
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
    }
}
