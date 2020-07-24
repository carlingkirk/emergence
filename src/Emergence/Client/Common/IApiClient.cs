using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;

namespace Emergence.Client.Common
{
    public interface IApiClient
    {
        Task<IEnumerable<Origin>> FindOriginsAsync(string searchText, int? skip = 0, int? take = 10);
        Task<IEnumerable<Lifeform>> FindLifeformsAsync(string searchText, int? skip = 0, int? take = 10);
        Task<IEnumerable<Specimen>> FindSpecimensAsync(string searchText, int? skip = 0, int? take = 10);
        Task<IEnumerable<Activity>> FindActivitiesAsync(string searchText, int? skip = 0, int? take = 10);
        Task<IEnumerable<PlantInfo>> FindPlantInfosAsync(string searchText, int? skip = 0, int? take = 10);
        Task<Specimen> GetSpecimenAsync(int id);
        Task<Specimen> PutSpecimenAsync(Specimen specimen);
        Task<PlantInfo> GetPlantInfoAsync(int id);
        Task<PlantInfo> PutPlantInfoAsync(PlantInfo plantInfo);
        Task<Activity> GetActivityAsync(int id);
        Task<Activity> PutActivityAsync(Activity activity);
        Task<Origin> GetOriginAsync(int id);
        Task<Origin> PutOriginAsync(Origin origin);
    }
}
