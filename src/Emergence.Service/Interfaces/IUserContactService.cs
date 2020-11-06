using System.Threading.Tasks;
using Emergence.Data.Shared.Models;

namespace Emergence.Service.Interfaces
{
    public interface IUserContactService
    {
        public Task<UserContact> GetUserContact(User requestor, Data.Shared.Stores.User user);
    }
}
