using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Emergence.Service.Interfaces
{
    public interface IBlobService
    {
        Task<IEnumerable<IBlobResult>> UploadPhotosAsync(IEnumerable<IFormFile> photos, string path, string name);
        Task<IBlobResult> UploadPhotoAsync(IFormFile photo, string path, string name);
    }
}
