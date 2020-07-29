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
        Task<IEnumerable<Photo>> AddOrUpdatePhotosAsync(IEnumerable<Photo> photos);
        Task<Photo> GetPhotoAsync(int id);
        Task<IEnumerable<Photo>> GetPhotosAsync(IEnumerable<int> ids);
        Task<IEnumerable<Photo>> GetPhotosAsync(PhotoType type, int typeId);
        Task<bool> RemovePhotoAsync(int id, string userId);
    }
}
