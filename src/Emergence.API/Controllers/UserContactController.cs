using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;
using Emergence.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Emergence.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserContactController : BaseAPIController
    {
        private readonly IUserContactService _userContactService;
        private readonly IUserService _userService;

        public UserContactController(IUserContactService userContactService, IUserService userService)
        {
            _userContactService = userContactService;
            _userService = userService;
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IEnumerable<UserContact>> GetUserContacts(int userId) => await _userContactService.GetUserContactsAsync(userId);

        [HttpGet]
        [Route("requests/{userId}")]
        public async Task<IEnumerable<UserContact>> GetUserContactRequests(int userId) => await _userContactService.GetUserContactsAsync(userId);

        [HttpPost]
        public async Task<UserContact> AddUserContact(UserContactRequest userContactRequest) =>
            await _userContactService.AddUserContactAsync(userContactRequest);

        [HttpPost]
        [Route("request")]
        public async Task<UserContactRequest> AddUserContactRequest(UserContactRequest userContactRequest)
        {
            var user = await _userService.GetIdentifyingUser(UserId);
            userContactRequest.ContactUserId = user.Id;
            userContactRequest.DateRequested = DateTime.UtcNow;

            return await _userContactService.AddUserContactRequestAsync(userContactRequest);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<bool> RemoveUserContact(int id) => await _userContactService.RemoveUserContactRequestAsync(id);

        [HttpDelete]
        [Route("request/{id}")]
        public async Task<bool> RemoveUserContactRequest(int id) => await _userContactService.RemoveUserContactRequestAsync(id);
    }
}
