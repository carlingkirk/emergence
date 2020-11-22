using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Extensions;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;
using Emergence.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Emergence.Service
{
    public class MessageService : IMessageService
    {
        private readonly IRepository<UserMessage> _userMessageRepository;
        public MessageService(IRepository<UserMessage> userMessageRepository)
        {
            _userMessageRepository = userMessageRepository;
        }
        public async Task<Data.Shared.Models.UserMessage> SendMessageAsync(Data.Shared.Models.UserMessage message)
        {
            var messageResult = await _userMessageRepository.AddOrUpdateAsync(m => m.Id == message.Id, message.AsStore());
            return messageResult.AsModel();
        }

        public async Task<IEnumerable<Data.Shared.Models.UserMessage>> GetMessagesAsync(int userId)
        {
            var userMessages = new List<Data.Shared.Models.UserMessage>();
            var userMessageResult = _userMessageRepository.GetSomeAsync(uc => uc.UserId == userId);

            await foreach (var userMessage in userMessageResult)
            {
                userMessages.Add(userMessage.AsModel());
            }

            return userMessages;
        }

        public async Task<IEnumerable<Data.Shared.Models.UserMessage>> GetSentMessagesAsync(int senderId)
        {
            var userMessages = new List<Data.Shared.Models.UserMessage>();
            var userMessageResult = _userMessageRepository.GetSomeAsync(uc => uc.SenderId == senderId);

            await foreach (var userMessage in userMessageResult)
            {
                userMessages.Add(userMessage.AsModel());
            }

            return userMessages;
        }

        public async Task<FindResult<Data.Shared.Models.UserMessage>> FindMessages(FindParams findParams, string userId)
        {
            var messageQuery = _userMessageRepository.WhereWithIncludes(um => um.User.UserId == userId &&
                                                                              (findParams.SearchTextQuery == null ||
                                                                               EF.Functions.Like(um.Sender.DisplayName, findParams.SearchTextQuery) ||
                                                                               EF.Functions.Like(um.Subject, findParams.SearchTextQuery) ||
                                                                               EF.Functions.Like(um.MessageBody, findParams.SearchTextQuery)),
                                                                        false,
                                                                        um => um.Include(um => um.User),
                                                                        um => um.Include(um => um.Sender));

            messageQuery = OrderBy(messageQuery, findParams.SortBy, findParams.SortDirection);

            var count = messageQuery.Count();
            var userMessageResult = messageQuery.GetSomeAsync(skip: findParams.Skip, take: findParams.Take, track: false);

            var userMessages = new List<Data.Shared.Models.UserMessage>();
            await foreach (var userMessage in userMessageResult)
            {
                userMessages.Add(userMessage.AsModel());
            }

            return new FindResult<Data.Shared.Models.UserMessage>
            {
                Count = count,
                Results = userMessages
            };
        }

        public async Task<FindResult<Data.Shared.Models.UserMessage>> FindSentMessages(FindParams findParams, string senderId)
        {
            var messageQuery = _userMessageRepository.WhereWithIncludes(um => um.Sender.UserId == senderId &&
                                                                              (findParams.SearchTextQuery == null ||
                                                                               EF.Functions.Like(um.User.DisplayName, findParams.SearchTextQuery) ||
                                                                               EF.Functions.Like(um.Subject, findParams.SearchTextQuery) ||
                                                                               EF.Functions.Like(um.MessageBody, findParams.SearchTextQuery)),
                                                                        false,
                                                                        um => um.Include(um => um.User),
                                                                        um => um.Include(um => um.Sender));

            messageQuery = OrderBy(messageQuery, findParams.SortBy, findParams.SortDirection);

            var count = messageQuery.Count();
            var userMessageResult = messageQuery.GetSomeAsync(skip: findParams.Skip, take: findParams.Take, track: false);

            var userMessages = new List<Data.Shared.Models.UserMessage>();
            await foreach (var userMessage in userMessageResult)
            {
                userMessages.Add(userMessage.AsModel());
            }

            return new FindResult<Data.Shared.Models.UserMessage>
            {
                Count = count,
                Results = userMessages
            };
        }

        private IQueryable<UserMessage> OrderBy(IQueryable<UserMessage> messageQuery, string sortBy = null, SortDirection sortDirection = SortDirection.None)
        {
            if (sortDirection == SortDirection.None)
            {
                return messageQuery;
            }
            if (sortBy == null)
            {
                sortBy = "DateSent";
            }
            var messageSorts = new Dictionary<string, Expression<Func<UserMessage, object>>>
            {
                { "DisplayName", um => um.User.DisplayName },
                { "SenderName", um => um.Sender.DisplayName },
                { "Subject", um => um.Subject },
                { "MessageBody", um => um.MessageBody },
                { "DateSent", um => um.DateSent }
            };

            if (sortDirection == SortDirection.Descending)
            {
                messageQuery = messageQuery.WithOrder(a => a.OrderByDescending(messageSorts[sortBy]));
            }
            else
            {
                messageQuery = messageQuery.WithOrder(a => a.OrderBy(messageSorts[sortBy]));
            }

            return messageQuery;
        }
    }
}
