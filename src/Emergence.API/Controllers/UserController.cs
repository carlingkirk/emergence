using System.Threading.Tasks;
using Emergence.Data.Shared.Models;
using Emergence.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Emergence.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseAPIController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("{userId}")]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<User> Get(string userId) => await _userService.GetUserAsync(userId);

        [HttpGet]
        [Route("get")]
        public async Task<User> Get(int? id, string name)
        {
            var viewingUser = await _userService.GetIdentifyingUser(UserId);
            User user;
            if (id.HasValue)
            {
                user = await _userService.GetUserAsync(id.Value, viewingUser);
            }
            else
            {
                user = await _userService.GetUserByNameAsync(name, viewingUser);
            }

            return user ?? new User { DisplayName = "Anonymous user" };
        }
    }
}
