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
    public class OriginController : BaseApiController
    {
        private readonly IOriginService _originService;
        private readonly IUserService _userService;
        private readonly IPhotoService _photoService;

        public OriginController(IOriginService originService, IUserService userService, IPhotoService photoService)
        {
            _originService = originService;
            _userService = userService;
            _photoService = photoService;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{id}")]
        public async Task<Origin> Get(int id)
        {
            var user = await _userService.GetIdentifyingUser(UserId);
            var origin = await _originService.GetOriginAsync(id, user);

            if (origin.User?.PhotoId != null)
            {
                var userPhoto = await _photoService.GetPhotoAsync(origin.User.PhotoId.Value);
                origin.User.PhotoThumbnailUri = userPhoto.ThumbnailUri;
            }

            return origin;
        }

        [HttpPut]
        public async Task<Origin> Put(Origin origin)
        {
            origin.UserId = await _userService.GetUserIdAsync(UserId);
            return await _originService.AddOrUpdateOriginAsync(origin, UserId);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Find")]
        public async Task<FindResult<Origin>> FindOrigins(FindParams findParams)
        {
            var user = await _userService.GetIdentifyingUser(UserId);
            var result = await _originService.FindOrigins(findParams, user);
            return result;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetIdentifyingUser(UserId);
            var origin = await _originService.GetOriginAsync(id, user);
            if (origin.CreatedBy != UserId)
            {
                return Unauthorized();
            }

            await _originService.RemoveOriginAsync(origin);

            return Ok();
        }
    }
}
