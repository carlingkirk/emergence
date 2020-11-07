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
        public UserContactController(IUserContactService userContactService)
        {
            _userContactService = userContactService;
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IEnumerable<UserContact>> GetUserContacts(int userId) => await _userContactService.GetUserContacts(userId);

        [HttpGet]
        [Route("requests/{userId}")]
        public async Task<IEnumerable<UserContact>> GetUserContactRequests(int userId) => await _userContactService.GetUserContacts(userId);

        [HttpPost]
        public async Task<UserContact> AddUserContact(UserContactRequest userContactRequest) => await _userContactService.AddUserContact(userContactRequest);

        [HttpPost]
        [Route("request")]
        public async Task<UserContactRequest> AddUserContactRequest(UserContactRequest userContactRequest) => await _userContactService.AddUserContactRequest(userContactRequest);

        [HttpDelete]
        [Route("{id}")]
        public async Task<bool> RemoveUserContact(int id) => await _userContactService.RemoveUserContactRequest(id);

        [HttpDelete]
        [Route("request/{id}")]
        public async Task<bool> RemoveUserContactRequest(int id) => await _userContactService.RemoveUserContactRequest(id);
    }
}
