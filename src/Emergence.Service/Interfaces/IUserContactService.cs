using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;

namespace Emergence.Service.Interfaces
{
    public interface IUserContactService
    {
        public Task<UserContact> GetUserContact(User requestor, Data.Shared.Stores.User user);
        public Task<IEnumerable<UserContact>> GetUserContacts(int userId);
        public Task<IEnumerable<UserContactRequest>> GetUserContactRequests(int userId);
        public Task<UserContact> AddUserContact(UserContactRequest request);
        public Task<bool> RemoveUserContactRequest(int id);
        public Task<bool> RemoveUserContact(int id);
        public Task<UserContactRequest> AddUserContactRequest(UserContactRequest userContact);
    }
}
