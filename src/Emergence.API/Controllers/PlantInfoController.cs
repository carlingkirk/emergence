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
        public PlantInfoController(IPlantInfoService plantInfoService)
        {
            _plantInfoService = plantInfoService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<PlantInfo> Get(int id) => await _plantInfoService.GetPlantInfoAsync(id);

        [HttpPut]
        public async Task<PlantInfo> Put(PlantInfo plantInfo) => await _plantInfoService.AddOrUpdatePlantInfoAsync(plantInfo);

        [HttpGet]
        [Route("Find")]
        public async Task<IEnumerable<PlantInfo>> FindPlantInfos(string search = null, int skip = 0, int take = 10)
        {
            var results = await _plantInfoService.FindPlantInfos(search, skip, take);
            return results;
        }
    }
}
