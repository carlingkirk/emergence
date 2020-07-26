using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Http;

namespace Emergence.Service.Interfaces
{
    public interface IPhotoService
    {
        Task<IEnumerable<Photo>> UploadPhotosAsync(IEnumerable<IFormFile> photos, PhotoType type, string userId);
        Task<Photo> AddOrUpdatePhotoAsync(Photo photo);
        Task<bool> AddOrUpdatePhotosAsync(IEnumerable<Photo> photos);
        Task<IEnumerable<Photo>> GetPhotosAsync(IEnumerable<int> ids);
        Task<IEnumerable<Photo>> GetPhotosAsync(PhotoType type, int typeId);
    }
}
