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
        private readonly IRepository<DisplayName> _nameRepository;
        private readonly IPhotoService _photoService;
        private readonly ILocationService _locationService;

        public UserService(IRepository<User> userRepository, IRepository<DisplayName> nameRepository, IPhotoService photoService, ILocationService locationService)
        {
            _userRepository = userRepository;
            _nameRepository = nameRepository;
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
            var userModel = userResult.AsModel();

            if (userResult.PhotoId.HasValue)
            {
                var photo = await _photoService.GetPhotoAsync(userResult.PhotoId.Value);
                userModel.Photo = photo;
            }

            return userModel;
        }

        public async Task<Data.Shared.Models.User> GetUserByNameAsync(string name)
        {
            var userResult = await _userRepository.GetWithIncludesAsync(u => u.DisplayName == name, false, u => u.Include(u => u.Location));
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

        public async Task<Data.Shared.Models.User> AddUserAsync(Data.Shared.Models.User user)
        {
            var userResult = await _userRepository.AddOrUpdateAsync(u => u.Id == user.Id, user.AsStore());

            return userResult.AsModel();
        }

        public async Task<string> GetRandomNameAsync()
        {
            var result = await _nameRepository.GetAsync(n => true);
            return result.Name;
        }
    }
}
