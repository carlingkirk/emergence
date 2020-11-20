using System.Linq;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Extensions;
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
        private readonly IUserContactService _userContactService;
        private readonly IPhotoService _photoService;
        public UserController(IUserService userService, IUserContactService userContactService, IPhotoService photoService)
        {
            _userService = userService;
            _userContactService = userContactService;
            _photoService = photoService;
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

            return await GetVisibleUser(user, viewingUser);
        }

        [HttpPost]
        [Route("find")]
        public async Task<FindResult<UserSummary>> FindUsers(FindParams findParams)
        {
            var result = await _userService.FindUsers(findParams, UserId);

            return result;
        }

        private async Task<User> GetVisibleUser(User user, User requestor)
        {
            if (user != null)
            {
                user = await _userContactService.GetUserContactStatusAsync(requestor, user);

                if (!requestor.CanViewUser(user))
                {
                    user = new User
                    {
                        Id = user.Id,
                        UserId = user.UserId,
                        DisplayName = user.DisplayName,
                        Photo = user.Photo,
                        ProfileVisibility = user.ProfileVisibility,
                        Contacts = user.Contacts,
                        ContactRequests = user.ContactRequests
                    };
                }

                if (user.Photo != null)
                {
                    var photo = await _photoService.GetPhotoAsync(user.Photo.PhotoId);
                    user.Photo = photo;
                }

                user.IsViewerContact = user.Contacts != null && user.Contacts.Any(c => c.ContactUserId == requestor.Id);
                user.IsViewerContactRequested = user.ContactRequests != null && user.ContactRequests.Any(c => c.ContactUserId == requestor.Id);

                return user;
            }

            return null;
        }
    }
}
