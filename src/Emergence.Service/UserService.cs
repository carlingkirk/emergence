using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Extensions;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;
using Emergence.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Emergence.Service
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<DisplayName> _nameRepository;
        private readonly IPhotoService _photoService;
        private readonly IUserContactService _userContactService;
        private readonly ILocationService _locationService;
        private readonly ICacheService _cacheService;

        public UserService(IRepository<User> userRepository, IRepository<DisplayName> nameRepository, IPhotoService photoService, ILocationService locationService,
            ICacheService cacheService, IUserContactService userContactService)
        {
            _userRepository = userRepository;
            _nameRepository = nameRepository;
            _photoService = photoService;
            _userContactService = userContactService;
            _locationService = locationService;
            _cacheService = cacheService;
        }

        public async Task<Data.Shared.Models.User> GetUserAsync(int id, Data.Shared.Models.User requestor)
        {
            var user = await _userRepository.GetWithIncludesAsync(u => u.Id == id, false, u => u.Include(u => u.Location));

            return await GetVisibleUser(user, requestor);
        }

        public async Task<Data.Shared.Models.User> GetUserAsync(string userId)
        {
            var userResult = await _userRepository.GetWithIncludesAsync(u => u.UserId == userId, false, u => u.Include(u => u.Location));
            if (userResult != null)
            {
                var userModel = userResult.AsModel();

                if (userResult.PhotoId.HasValue)
                {
                    var photo = await _photoService.GetPhotoAsync(userResult.PhotoId.Value);
                    userModel.Photo = photo;
                }

                return userModel;
            }

            return new Data.Shared.Models.User
            {
                UserId = userId
            };
        }

        public async Task<Data.Shared.Models.User> GetUserByNameAsync(string name, Data.Shared.Models.User requestor)
        {
            var user = await _userRepository.GetWithIncludesAsync(u => u.DisplayName == name, false, u => u.Include(u => u.Location));

            return await GetVisibleUser(user, requestor);
        }

        public async Task<int?> GetUserIdAsync(string userId)
        {
            var cacheKey = "UserId:" + userId + ":Id";
            var id = await _cacheService.GetIntAsync(cacheKey);

            if (id != null)
            {
                return id.Value;
            }
            else
            {
                var userResult = await _userRepository.GetAsync(u => u.UserId == userId, false);
                if (userResult != null)
                {
                    await _cacheService.SetCacheValueAsync<int>(cacheKey, userResult.Id);
                    return userResult.Id;
                }
            }

            return null;
        }

        public async Task<Data.Shared.Models.User> GetIdentifyingUser(string userId)
        {
            var id = await GetUserIdAsync(userId);
            return new Data.Shared.Models.User { Id = id ?? default, UserId = userId };
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

        private async Task<Data.Shared.Models.User> GetVisibleUser(User user, Data.Shared.Models.User requestor)
        {
            Data.Shared.Models.User userModel;
            Data.Shared.Models.UserContact userContact = null;
            if (user != null)
            {
                if (user.ProfileVisibility == Data.Shared.Visibility.Contacts)
                {
                    userContact = await _userContactService.GetUserContact(requestor, user);
                    if (userContact != null)
                    {
                        user.Contacts = new List<UserContact>
                        {
                            userContact.AsStore()
                        };
                    }
                }

                if (requestor.CanViewUser(user))
                {
                    userModel = user.AsModel();
                }
                else
                {
                    userModel = new Data.Shared.Models.User
                    {
                        Id = user.Id,
                        UserId = user.UserId,
                        DisplayName = user.DisplayName,
                        Photo = user.Photo?.AsModel(),
                        ProfileVisibility = user.ProfileVisibility
                    };
                }

                if (userModel.Photo != null)
                {
                    var photo = await _photoService.GetPhotoAsync(user.PhotoId.Value);
                    userModel.Photo = photo;
                }

                userModel.IsViewerContact = userContact != null;

                return userModel;
            }

            return null;
        }
    }
}
