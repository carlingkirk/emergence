using System.Collections.Generic;
using System.Threading.Tasks;
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
            if (plantInfo.Origin != null && plantInfo.Origin.OriginId == 0)
            {
                plantInfo.Origin = await _originService.AddOrUpdateOriginAsync(plantInfo.Origin);
            }

            return await _plantInfoService.AddOrUpdatePlantInfoAsync(plantInfo);
        }

        [HttpGet]
        [Route("Find")]
        public async Task<IEnumerable<PlantInfo>> FindPlantInfos(string search = null, int skip = 0, int take = 10)
        {
            var results = await _plantInfoService.FindPlantInfos(search, skip, take);
            return results;
        }
    }
}
