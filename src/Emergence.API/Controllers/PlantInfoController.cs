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
    public class PlantInfoController : BaseAPIController
    {
        private readonly IPlantInfoService _plantInfoService;
        private readonly IOriginService _originService;
        private readonly IPhotoService _photoService;

        public PlantInfoController(IPlantInfoService plantInfoService, IOriginService originService, IPhotoService photoService)
        {
            _plantInfoService = plantInfoService;
            _originService = originService;
            _photoService = photoService;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{id}")]
        public async Task<PlantInfo> Get(int id)
        {
            var plantInfo = await _plantInfoService.GetPlantInfoAsync(id);
            var photos = await _photoService.GetPhotosAsync(PhotoType.PlantInfo, plantInfo.PlantInfoId);
            plantInfo.Photos = photos;

            return plantInfo;
        }

        [HttpPut]
        public async Task<PlantInfo> Put(PlantInfo plantInfo)
        {
            if (plantInfo.Origin != null && plantInfo.Origin.OriginId == 0 && (!string.IsNullOrEmpty(plantInfo.Origin.Name) || plantInfo.Origin.Uri != null))
            {
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
            var result = await _plantInfoService.FindPlantInfos(findParams);
            return result;
        }
    }
}
