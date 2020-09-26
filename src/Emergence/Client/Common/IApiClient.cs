using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorInputFile;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Enums;
using Emergence.Data.Shared.Models;

namespace Emergence.Client.Common
{
    public interface IApiClient
    {
        Task<FindResult<Activity>> FindActivitiesAsync(FindParams findParams);
        Task<FindResult<Activity>> FindActivitiesAsync(Specimen specimen, FindParams findParams);
        Task<FindResult<Lifeform>> FindLifeformsAsync(FindParams findParams);
        Task<FindResult<Origin>> FindOriginsAsync(FindParams findParams);
        Task<FindResult<PlantInfo>> FindPlantInfosAsync(FindParams findParams);
        Task<FindResult<Specimen>> FindSpecimensAsync(FindParams findParams);
        Task<FindResult<Taxon>> FindTaxonsAsync(FindParams<Taxon> findParams, TaxonRank rank);
        Task<Lifeform> GetLifeformAsync(int id);
        Task<Specimen> GetSpecimenAsync(int id);
        Task<Specimen> PutSpecimenAsync(Specimen specimen);
        Task<bool> RemoveSpecimenAsync(Specimen specimen);
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
