using System;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;
using Emergence.Service.Interfaces;

namespace Emergence.Service
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Data.Shared.Models.User> GetUser(string id)
        {
            var userId = new Guid(id);
            var user = await _userRepository.GetAsync(u => u.Id == userId);
            return user.AsModel();
        }
    }
}
