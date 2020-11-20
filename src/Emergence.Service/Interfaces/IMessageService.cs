using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Service.Interfaces
{
    public interface IMessageService
    {
        Task<UserMessage> SendMessageAsync(UserMessage message);
        Task<IEnumerable<UserMessage>> GetSentMessagesAsync(int senderId);
        Task<IEnumerable<UserMessage>> GetMessagesAsync(int userId);
        Task<FindResult<UserMessage>> FindMessages(FindParams findParams, string userId);
        Task<FindResult<UserMessage>> FindSentMessages(FindParams findParams, string senderId);
    }
}
