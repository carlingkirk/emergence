using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;
using Emergence.Service.Interfaces;
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
        [Route("{id}")]
        public async Task<Lifeform> Get(int id) => await _lifeformService.GetLifeformAsync(id);

        [HttpPut]

        public async Task<Lifeform> Put(Lifeform lifeform) => await _lifeformService.AddOrUpdateLifeformAsync(lifeform);

        [HttpGet]
        [Route("Find")]
        public async Task<IEnumerable<Lifeform>> FindLifeforms(string search, int skip = 0, int take = 10)
        {
            var results = await _lifeformService.FindLifeforms(search, skip, take);
            return results;
        }
    }
}
