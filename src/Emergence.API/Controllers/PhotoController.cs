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
        private readonly ILocationService _locationService;
        private readonly IConfigurationService _configurationService;
        private readonly string _blobStorageRoot;

        public PhotoController(IPhotoService photoService, ILocationService locationService, IConfigurationService configurationService)
        {
            _photoService = photoService;
            _locationService = locationService;
            _configurationService = configurationService;
            _blobStorageRoot = _configurationService.Settings.BlobStorageRoot + "photos/";
        }

        [HttpPost]
        [Route("{type}/UploadMany")]
        public async Task<IEnumerable<Photo>> UploadMany(IEnumerable<IFormFile> photos, PhotoType type)
        {
            var photoResult = await _photoService.UploadOriginalsAsync(photos, type, UserId);
            var locations = photoResult.Select(p => p.Location).ToList();

            var locationResult = await _locationService.AddLocationsAsync(locations);

            foreach (var photo in photoResult.Where(p => p.Location != null))
            {
                var location = locationResult.Where(l => l.Latitude == photo.Location.Latitude && l.Longitude == photo.Location.Longitude).FirstOrDefault();
                photo.Location = location;
            }

            photoResult = (await _photoService.AddOrUpdatePhotosAsync(photoResult)).ToList();

            foreach (var photo in photoResult)
            {
                photo.BlobPathRoot = _blobStorageRoot;
            }
            return photoResult;
        }

        [HttpPost]
        [Route("{type}/Upload")]
        public async Task<Photo> Upload(IFormFile photo, PhotoType type)
        {
            var photoResults = await _photoService.UploadOriginalsAsync(new IFormFile[] { photo }, type, UserId);
            if (photoResults.Any())
            {
                var photoResult = photoResults.First();

                if (photoResult.Location != null)
                {
                    photoResult.Location = await _locationService.AddOrUpdateLocationAsync(photoResult.Location);
                }

                photoResult = await _photoService.AddOrUpdatePhotoAsync(photoResult);
                photoResult.BlobPathRoot = _blobStorageRoot;
                return photoResult;
            }
            return null;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _photoService.RemovePhotoAsync(id, UserId);
            {
                if (result)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        [HttpGet]
        [Route("{type}/{id}")]
        public async Task<IEnumerable<Photo>> Get(PhotoType type, int id)
        {
            var photoResult = await _photoService.GetPhotosAsync(type, id);

            foreach (var photo in photoResult)
            {
                photo.BlobPathRoot = _blobStorageRoot;
            }

            return photoResult;
        }
    }
}
