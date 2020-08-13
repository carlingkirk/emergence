using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Emergence.Service.Interfaces
{
    public interface IBlobService
    {
        Task<IBlobResult> UploadPhotoAsync(IFormFile photo, string userId, string blobPath);
        Task<bool> RemovePhotoAsync(string filename);
        Task<bool> UploadPhotoStreamAsync(MemoryStream stream, string blobPath);
        Task<IBlobResult> SetBlobPropertiesAsync(Stream stream, string blobPath, string userId, string contentType);
        Task<IBlobResult> GetBlobPropertiesAsync(string blobPath);
        Task<IBlobResult> SetBlobPropertiesAsync(IBlobResult blobProperties, string blobPath, string userId);
    }
}
