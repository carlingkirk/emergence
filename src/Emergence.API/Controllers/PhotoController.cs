using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;
using Emergence.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Emergence.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : BaseAPIController
    {
        private readonly IPhotoService _photoService;
        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpPost]
        [Route("{type}/UploadMany")]
        public async Task<IEnumerable<Photo>> UploadMany(IEnumerable<IFormFile> photos, PhotoType type)
        {
            var photoResult = await _photoService.UploadPhotosAsync(photos, type, UserId);
            await _photoService.AddOrUpdatePhotosAsync(photoResult);
            return await _photoService.GetPhotosAsync(photoResult.Select(p => p.PhotoId));
        }

        [HttpPost]
        [Route("{type}/Upload")]
        public async Task<Photo> Upload(IFormFile photo, PhotoType type)
        {
            var photoResult = await _photoService.UploadPhotosAsync(new IFormFile[] { photo }, type, UserId);
            await _photoService.AddOrUpdatePhotosAsync(photoResult);
            var result = await _photoService.GetPhotosAsync(photoResult.Select(p => p.PhotoId));
            return result.FirstOrDefault();
        }
    }
}
