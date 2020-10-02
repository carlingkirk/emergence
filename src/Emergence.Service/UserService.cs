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

        public async Task<Data.Shared.Models.User> GetUserAsync(int id)
        {
            var user = await _userRepository.GetAsync(u => u.Id == id);
            return user.AsModel();
        }

        public async Task<Data.Shared.Models.User> GetUserAsync(string userId)
        {
            var userGuid = new Guid(userId);
            var user = await _userRepository.GetAsync(u => u.UserId == userGuid);
            return user.AsModel();
        }

        public async Task<Data.Shared.Models.User> UpdateUserAsync(Data.Shared.Models.User user)
        {
            var userResult = await _userRepository.UpdateAsync(user.AsStore());
            return userResult.AsModel();
        }
    }
}
