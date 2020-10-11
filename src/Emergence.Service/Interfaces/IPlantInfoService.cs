using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Service.Interfaces
{
    public interface IPlantInfoService
    {
        Task<PlantInfo> AddOrUpdatePlantInfoAsync(PlantInfo plantInfo);
        Task<PlantInfo> GetPlantInfoAsync(int id);
        Task<PlantInfo> GetPlantInfoAsync(int originId, string scientificName);
        Task<PlantInfo> GetPlantInfoAsync(int originId, int taxonId);
        Task<IEnumerable<PlantInfo>> GetPlantInfosAsync();
        Task<FindResult<PlantInfo>> FindPlantInfos(FindParams findParams);
        Task<IEnumerable<PlantInfo>> AddPlantInfosAsync(IEnumerable<PlantInfo> plantInfos);
        Task<IEnumerable<PlantLocation>> AddPlantLocations(IEnumerable<PlantLocation> plantLocations);
        Task<bool> RemovePlantInfoAsync(PlantInfo plantInfo);
    }
}
