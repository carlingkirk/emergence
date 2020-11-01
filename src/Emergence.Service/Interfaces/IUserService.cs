using System.Threading.Tasks;
using Emergence.Data.Shared.Models;

namespace Emergence.Service.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserAsync(int id);
        Task<User> GetUserAsync(string userId);
        Task<User> GetUserByNameAsync(string name);
        Task<int?> GetUserIdAsync(string userId);
        Task<User> UpdateUserAsync(User user);
        Task<User> AddUserAsync(User user);
        Task<string> GetRandomNameAsync();
    }
}
