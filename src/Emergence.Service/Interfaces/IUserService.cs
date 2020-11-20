using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Service.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserAsync(int id, User requestor);
        Task<User> GetUserAsync(string userId);
        Task<User> GetUserByNameAsync(string name, User requestor);
        Task<int?> GetUserIdAsync(string userId);
        Task<User> GetIdentifyingUser(string userId);
        Task<User> UpdateUserAsync(User user);
        Task<User> AddUserAsync(User user);
        Task<string> GetRandomNameAsync();
        Task<FindResult<UserSummary>> FindUsers(FindParams findParams, string userId);
    }
}
