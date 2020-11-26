using System.Linq;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;
using Emergence.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Emergence.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantInfoController : BaseApiController
    {
        private readonly IPlantInfoService _plantInfoService;
        private readonly IOriginService _originService;
        private readonly IPhotoService _photoService;
        private readonly IUserService _userService;

        public PlantInfoController(IPlantInfoService plantInfoService, IOriginService originService, IPhotoService photoService, IUserService userService)
        {
            _plantInfoService = plantInfoService;
            _originService = originService;
            _photoService = photoService;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{id}")]
        public async Task<PlantInfo> Get(int id)
        {
            var user = await _userService.GetIdentifyingUser(UserId);
            var plantInfo = await _plantInfoService.GetPlantInfoAsync(id, user);

            if (plantInfo.User?.PhotoId != null)
            {
                var userPhoto = await _photoService.GetPhotoAsync(plantInfo.User.PhotoId.Value);
                plantInfo.User.PhotoThumbnailUri = userPhoto.ThumbnailUri;
            }

            var photos = await _photoService.GetPhotosAsync(PhotoType.PlantInfo, plantInfo.PlantInfoId);
            plantInfo.Photos = photos;

            return plantInfo;
        }

        [HttpPut]
        public async Task<PlantInfo> Put(PlantInfo plantInfo)
        {
            plantInfo.CreatedBy = UserId;
            var userId = await _userService.GetUserIdAsync(UserId);
            plantInfo.UserId = userId;

            if (plantInfo.Origin != null && plantInfo.Origin.OriginId == 0)
            {
                plantInfo.Origin.UserId = userId;
                plantInfo.Origin = await _originService.AddOrUpdateOriginAsync(plantInfo.Origin, UserId);
            }

            var plantInfoResult = await _plantInfoService.AddOrUpdatePlantInfoAsync(plantInfo);

            if (plantInfo.Photos != null && plantInfo.Photos.Any())
            {
                foreach (var photo in plantInfo.Photos)
                {
                    photo.TypeId = plantInfoResult.PlantInfoId;
                }

                plantInfoResult.Photos = await _photoService.AddOrUpdatePhotosAsync(plantInfo.Photos);
            }

            return plantInfoResult;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Find")]
        public async Task<FindResult<PlantInfo>> FindPlantInfos(FindParams findParams)
        {
            var user = await _userService.GetIdentifyingUser(UserId);
            var result = await _plantInfoService.FindPlantInfos(findParams, user);

            var typeIds = result.Results.Select(p => p.PlantInfoId).ToList();
            var photos = await _photoService.GetPhotosByTypeAsync(PhotoType.PlantInfo, typeIds);

            foreach (var photoGroup in photos.GroupBy(p => p.TypeId))
            {
                var plantInfo = result.Results.FirstOrDefault(p => p.PlantInfoId == photoGroup.Key);
                if (plantInfo != null)
                {
                    plantInfo.Photos = photoGroup.ToList();
                }
            }

            return result;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetIdentifyingUser(UserId);
            var plantInfo = await _plantInfoService.GetPlantInfoAsync(id, user);
            if (plantInfo.CreatedBy != UserId)
            {
                return Unauthorized();
            }

            var photos = await _photoService.GetPhotosAsync(PhotoType.PlantInfo, plantInfo.PlantInfoId);
            await _photoService.RemovePhotosAsync(photos);
            await _plantInfoService.RemovePlantInfoAsync(plantInfo);

            return Ok();
        }
    }
}
