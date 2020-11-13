using System.IO;
using System.Threading.Tasks;

namespace Emergence.Functions.Services
{
    public interface IBlobService
    {
        Task<bool> UploadPhotoStreamAsync(MemoryStream stream, string blobPath);
        Task<IBlobResult> GetBlobPropertiesAsync(string blobPath);
        Task<IBlobResult> SetBlobPropertiesAsync(IBlobResult blobProperties, string blobPath, string userId);
    }
}
