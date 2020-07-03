using System.Threading.Tasks;
using Emergence.API.Services.Interfaces;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Emergence.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LifeformController : BaseAPIController
    {
        private readonly ILifeformService _lifeformService;
        public LifeformController(ILifeformService lifeformService)
        {
            _lifeformService = lifeformService;
        }

        [HttpGet]
        public async Task<Lifeform> Get(int id) => await _lifeformService.GetLifeformAsync(id);

        [HttpPut]

        public async Task<Lifeform> Put(Lifeform lifeform) => await _lifeformService.AddOrUpdateLifeformAsync(lifeform);
    }
}
