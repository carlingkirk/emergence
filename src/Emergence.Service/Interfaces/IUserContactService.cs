using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Service.Interfaces
{
    public interface IUserContactService
    {
        public Task<User> GetUserContactStatusAsync(User requestor, User user);
        public Task<IEnumerable<UserContact>> GetUserContactsAsync(int userId);
        public Task<IEnumerable<UserContactRequest>> GetUserContactRequestsAsync(int userId);
        public Task<UserContact> AddUserContactAsync(UserContactRequest request);
        public Task<bool> RemoveUserContactRequestAsync(int id);
        public Task<bool> RemoveUserContactAsync(int id);
        public Task<UserContactRequest> AddUserContactRequestAsync(UserContactRequest userContact);
        Task<FindResult<UserContact>> FindUserContacts(FindParams findParams, string userId);
        Task<FindResult<UserContactRequest>> FindUserContactRequests(FindParams findParams, string userId);
    }
}
