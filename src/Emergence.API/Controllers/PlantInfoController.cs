using System.Threading.Tasks;
using Emergence.API.Services.Interfaces;
using Emergence.Data.Shared.Models;
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
        public async Task<PlantInfo> Get(int id) => await _plantInfoService.GetPlantInfoAsync(id);

        [HttpPut]

        public async Task<PlantInfo> Put(PlantInfo plantInfo) => await _plantInfoService.AddOrUpdatePlantInfoAsync(plantInfo);
    }
}
