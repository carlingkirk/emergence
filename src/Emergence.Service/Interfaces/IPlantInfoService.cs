using System.Collections.Generic;
using System.Threading.Tasks;

namespace Emergence.Service.Interfaces
{
    public interface IPlantInfoService
    {
        Task<Data.Shared.Models.PlantInfo> AddOrUpdatePlantInfoAsync(Data.Shared.Models.PlantInfo plantInfo);
        Task<Data.Shared.Models.PlantInfo> GetPlantInfoAsync(int id);
        Task<Data.Shared.Models.PlantInfo> GetPlantInfoAsync(int originId, string scientificName);
        Task<IEnumerable<Data.Shared.Models.PlantInfo>> GetPlantInfosAsync();
    }
}
