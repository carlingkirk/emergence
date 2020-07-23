using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Extensions;
using Emergence.Data.Shared.Extensions;
using Emergence.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Emergence.Service
{
    public class PlantInfoService : IPlantInfoService
    {
        private readonly IRepository<Data.Shared.Stores.PlantInfo> _plantInfoRepository;
        public PlantInfoService(IRepository<Data.Shared.Stores.PlantInfo> plantInfoRepository)
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
            var plantInfo = await _plantInfoRepository.GetAsync(l => l.Id == id);
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

        public async Task<IEnumerable<Data.Shared.Models.PlantInfo>> FindPlantInfos(string search, int skip, int take)
        {
            if (search != null)
            {
                search = "%" + search + "%";
            }

            var plantInfoResult = _plantInfoRepository.WhereWithIncludesAsync(p => (search == null ||
                                                                       EF.Functions.Like(p.CommonName, search) ||
                                                                       EF.Functions.Like(p.ScientificName, search) ||
                                                                        EF.Functions.Like(p.Lifeform.CommonName, search) ||
                                                                        EF.Functions.Like(p.Lifeform.ScientificName, search)),
                                                                        p => p.Include(p => p.Lifeform)
                                                                              .Include(p => p.Taxon)
                                                                              .Include(p => p.Origin))
                                                    .WithOrder(p => p.OrderByDescending(p => p.DateCreated))
                                                    .GetSomeAsync(skip: skip, take: take, track: false);

            var plantInfos = new List<Data.Shared.Models.PlantInfo>();
            await foreach (var plantInfo in plantInfoResult)
            {
                plantInfos.Add(plantInfo.AsModel());
            }
            return plantInfos;
        }
    }
}
