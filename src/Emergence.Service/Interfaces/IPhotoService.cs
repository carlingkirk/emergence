using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;

namespace Emergence.Service.Interfaces
{
    public interface IPhotoService
    {
        Task<IEnumerable<Photo>> UploadOriginalsAsync(IEnumerable<IFormFile> photos, PhotoType type, string userId, bool storeLocation = true);
        Task<Photo> UploadOriginalAsync(IFormFile photo, PhotoType type, string userId, bool storeLocation = true);
        Task<Photo> AddOrUpdatePhotoAsync(Photo photo);
        Task<IEnumerable<Photo>> AddOrUpdatePhotosAsync(IEnumerable<Photo> photos);
        Task<Photo> GetPhotoAsync(int id);
        Task<IEnumerable<Photo>> GetPhotosAsync(IEnumerable<int> ids);
        Task<IEnumerable<Photo>> GetPhotosAsync(PhotoType type, int typeId);
        Task<IEnumerable<Photo>> GetPhotosByTypeAsync(PhotoType photoType, List<int> typeIds);
        Task<bool> RemovePhotoAsync(int id, string userId);
        Task<Image> ProcessPhotoAsync(Stream stream, Image image, ImageSize imageSize);
        Task RemovePhotosAsync(IEnumerable<Photo> photos);
    }
}
