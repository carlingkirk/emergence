using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;
using Emergence.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Emergence.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantInfoController : BaseAPIController
    {
        private readonly IPlantInfoService _plantInfoService;
        private readonly IOriginService _originService;
        public PlantInfoController(IPlantInfoService plantInfoService, IOriginService originService)
        {
            _plantInfoService = plantInfoService;
            _originService = originService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<PlantInfo> Get(int id) => await _plantInfoService.GetPlantInfoAsync(id);

        [HttpPut]
        public async Task<PlantInfo> Put(PlantInfo plantInfo)
        {
            if (plantInfo.Origin != null && plantInfo.Origin.OriginId == 0 && !string.IsNullOrEmpty(plantInfo.Origin.Name))
            {
                plantInfo.Origin = await _originService.AddOrUpdateOriginAsync(plantInfo.Origin, UserId);
            }

            return await _plantInfoService.AddOrUpdatePlantInfoAsync(plantInfo);
        }

        [HttpGet]
        [Route("Find")]
        public async Task<FindResult<PlantInfo>> FindPlantInfos(string search = null, int skip = 0, int take = 10, string sortBy = null,
            SortDirection sortDir = SortDirection.Ascending)
        {
            var result = await _plantInfoService.FindPlantInfos(search, skip, take, sortBy, sortDir);
            return result;
        }
    }
}
