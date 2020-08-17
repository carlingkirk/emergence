using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorInputFile;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Client.Common
{
    public interface IApiClient
    {
        Task<FindResult<Origin>> FindOriginsAsync(string searchText, int? skip = 0, int? take = 10, string sortBy = null,
            SortDirection sortDirection = SortDirection.Ascending);
        Task<IEnumerable<Lifeform>> FindLifeformsAsync(string searchText, int? skip = 0, int? take = 10);
        Task<FindResult<Specimen>> FindSpecimensAsync(string searchText, int? skip = 0, int? take = 10, string sortBy = null,
            SortDirection sortDirection = SortDirection.Ascending);
        Task<FindResult<Activity>> FindActivitiesAsync(Specimen specimen, FindParams findParams);
        Task<FindResult<Activity>> FindActivitiesAsync(FindParams findParams);
        Task<FindResult<PlantInfo>> FindPlantInfosAsync(string searchText, int? skip = 0, int? take = 10, string sortBy = null,
            SortDirection sortDirection = SortDirection.Ascending);
        Task<Specimen> GetSpecimenAsync(int id);
        Task<Specimen> PutSpecimenAsync(Specimen specimen);
        Task<PlantInfo> GetPlantInfoAsync(int id);
        Task<PlantInfo> PutPlantInfoAsync(PlantInfo plantInfo);
        Task<Activity> GetActivityAsync(int id);
        Task<Activity> PutActivityAsync(Activity activity);
        Task<Origin> GetOriginAsync(int id);
        Task<IEnumerable<Photo>> GetPhotosAsync(PhotoType type, int id);
        Task<Origin> PutOriginAsync(Origin origin);
        Task<IEnumerable<Photo>> UploadPhotosAsync(IFileListEntry[] photos, PhotoType type);
        Task<Photo> UploadPhotoAsync(IFileListEntry photo, PhotoType type);
        Task<bool> RemovePhotoAsync(int id);
    }
}
