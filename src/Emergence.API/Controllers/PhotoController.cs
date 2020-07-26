using System.Collections.Generic;
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
        [HttpPut]
        public async Task<IEnumerable<Photo>> Upload(IEnumerable<FormFile> photos, PhotoType type)
        {
            return await _photoService.UploadPhotosAsync(photos, type, UserId);
        }
    }
}
