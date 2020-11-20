using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;
using Emergence.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Emergence.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : BaseAPIController
    {
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;

        public MessageController(IMessageService messageService, IUserService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IEnumerable<UserMessage>> GetUserMessages(int userId)
        {
            var user = await _userService.GetIdentifyingUser(UserId);
            if (user.Id != userId)
            {
                return new List<UserMessage>();
            }

            return await _messageService.GetMessagesAsync(userId);
        }

        [HttpGet]
        [Route("sent/{userId}")]
        public async Task<IEnumerable<UserMessage>> GetSentMessages(int userId)
        {
            var user = await _userService.GetIdentifyingUser(UserId);
            if (user.Id != userId)
            {
                return new List<UserMessage>();
            }

            return await _messageService.GetSentMessagesAsync(userId);
        }

        [HttpPost]
        public async Task<UserMessage> SendMessage(UserMessage message)
        {
            var user = await _userService.GetIdentifyingUser(UserId);
            message.Sender = new UserSummary
            {
                Id = user.Id
            };
            message.DateSent = DateTime.UtcNow;
            return await _messageService.SendMessageAsync(message);
        }

        [HttpPost]
        [Route("find")]
        public async Task<FindResult<UserMessage>> FindUserMessages(FindParams findParams)
        {
            var result = await _messageService.FindMessages(findParams, UserId);

            return result;
        }

        [HttpPost]
        [Route("request/find")]
        public async Task<FindResult<UserMessage>> FindUserContactRequests(FindParams findParams)
        {
            var result = await _messageService.FindSentMessages(findParams, UserId);

            return result;
        }
    }
}
