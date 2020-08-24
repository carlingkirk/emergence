using System;
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
    public class OriginController : BaseAPIController
    {
        private readonly IOriginService _originService;
        public OriginController(IOriginService originService)
        {
            _originService = originService;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{id}")]
        public async Task<Origin> Get(int id) => await _originService.GetOriginAsync(id);

        [HttpPut]
        public async Task<Origin> Put(Origin origin)
        {
            if (!string.IsNullOrEmpty(origin.Name))
            {
                origin.UserId = UserId;
                return await _originService.AddOrUpdateOriginAsync(origin, UserId);
            }
            else
            {
                throw new InvalidOperationException("Origin name cannot be blank.");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Find")]
        public async Task<FindResult<Origin>> FindOrigins(FindParams findParams)
        {
            var result = await _originService.FindOrigins(findParams, UserId);
            return result;
        }
    }
}
