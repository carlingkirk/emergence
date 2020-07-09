using System.Collections.Generic;
using System.Threading.Tasks;

namespace Emergence.API.Services.Interfaces
{
    public interface IPlantInfoService
    {
        Task<Data.Shared.Models.PlantInfo> AddOrUpdatePlantInfoAsync(Data.Shared.Models.PlantInfo plantInfo);
        Task<Data.Shared.Models.PlantInfo> GetPlantInfoAsync(int id);
        Task<IEnumerable<Data.Shared.Models.PlantInfo>> GetPlantInfosAsync();
    }
}
