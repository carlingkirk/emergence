using System.Threading.Tasks;
using Emergence.Data.Shared.Models;

namespace Emergence.Service.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUser(string id);
    }
}
