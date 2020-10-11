using System;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Extensions;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;
using Emergence.Service.Interfaces;

namespace Emergence.Service
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IPhotoService _photoService;
        private readonly ILocationService _locationService;

        public UserService(IRepository<User> userRepository, IPhotoService photoService, ILocationService locationService)
        {
            _userRepository = userRepository;
            _photoService = photoService;
            _locationService = locationService;
        }

        public async Task<Data.Shared.Models.User> GetUserAsync(int id)
        {
            var userResult = await _userRepository.GetWithIncludesAsync(u => u.Id == id, false, u => u.Include(u => u.Location));
            var userModel = userResult.AsModel();

            if (userResult.PhotoId.HasValue)
            {
                var photo = await _photoService.GetPhotoAsync(userResult.PhotoId.Value);
                userModel.Photo = photo;
            }

            return userModel;
        }

        public async Task<Data.Shared.Models.User> GetUserAsync(string userId)
        {
            var userGuid = new Guid(userId);
            var userResult = await _userRepository.GetWithIncludesAsync(u => u.UserId == userGuid, false, u => u.Include(u => u.Location));
            if (userResult == null)
            {
                userResult = new User
                {
                    UserId = new Guid(userId),
                    DateCreated = DateTime.UtcNow
                };
                userResult = await _userRepository.UpdateAsync(userResult);
            }

            var userModel = userResult.AsModel();

            if (userResult.PhotoId.HasValue)
            {
                var photo = await _photoService.GetPhotoAsync(userResult.PhotoId.Value);
                userModel.Photo = photo;
            }

            return userModel;
        }

        public async Task<Data.Shared.Models.User> UpdateUserAsync(Data.Shared.Models.User user)
        {
            user.Location = await _locationService.AddOrUpdateLocationAsync(user.Location ??
                new Data.Shared.Models.Location
                {
                    DateCreated = DateTime.UtcNow,
                    CreatedBy = user.UserId.ToString()
                });

            var userResult = await _userRepository.UpdateAsync(user.AsStore());
            var userModel = userResult.AsModel();
            if (userResult.PhotoId.HasValue)
            {
                var photo = await _photoService.GetPhotoAsync(userResult.PhotoId.Value);
                userModel.Photo = photo;
            }

            return userModel;
        }
    }
}
