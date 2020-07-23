using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;
using Emergence.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Emergence.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OriginController : BaseAPIController
    {
        private readonly IOriginService _originService;
        public OriginController(IOriginService originService)
        {
            _originService = originService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Origin> Get(int id) => await _originService.GetOriginAsync(id);

        [HttpPut]
        public async Task<Origin> Put(Origin origin)
        {
            origin.UserId = UserId;
            return await _originService.AddOrUpdateOriginAsync(origin);
        }

        [HttpGet]
        [Route("Find")]
        public async Task<IEnumerable<Origin>> FindOrigins(string search, int skip = 0, int take = 10)
        {
            var results = await _originService.FindOrigins(search, UserId, skip, take);
            return results;
        }
    }
}
