using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.API.Services.Interfaces;
using Emergence.Data;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;

namespace Emergence.API.Services
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
            var plantInfoResult = await _plantInfoRepository.AddOrUpdateAsync(l => l.Id == plantInfo.PlantInfoId, plantInfo.AsStore());
            return plantInfoResult.AsModel();
        }

        public async Task<Data.Shared.Models.PlantInfo> GetPlantInfoAsync(int id)
        {
            var plantInfo = await _plantInfoRepository.GetAsync(l => l.Id == id);
            return plantInfo.AsModel();
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
    }
}
