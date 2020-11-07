using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;
using Emergence.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;

namespace Emergence.Service
{
    public class UserContactService : IUserContactService
    {
        private readonly IRepository<UserContact> _userContactRepository;
        private readonly IRepository<UserContactRequest> _userContactRequestRepository;
        public UserContactService(IRepository<UserContact> userContactRepository, IRepository<UserContactRequest> userContactRequestRepository)
        {
            _userContactRepository = userContactRepository;
            _userContactRequestRepository = userContactRequestRepository;
        }

        public async Task<Data.Shared.Models.UserContact> GetUserContact(Data.Shared.Models.User requestor, User user)
        {
            var userContact = await _userContactRepository.Where(uc => uc.UserId == user.Id && uc.ContactUserId == requestor.Id)
                .FirstOrDefaultAsync();

            return userContact.AsModel();
        }

        public async Task<IEnumerable<Data.Shared.Models.UserContact>> GetUserContacts(int userId)
        {
            var userContacts = new List<Data.Shared.Models.UserContact>();
            var userContactResult = _userContactRepository.GetSomeAsync(uc => uc.UserId == userId);

            await foreach (var userContact in userContactResult)
            {
                userContacts.Add(userContact.AsModel());
            }

            return userContacts;
        }

        public async Task<IEnumerable<Data.Shared.Models.UserContactRequest>> GetUserContactRequests(int userId)
        {
            var userContactRequests = new List<Data.Shared.Models.UserContactRequest>();
            var userContactRequestResult = _userContactRequestRepository.GetSomeAsync(uc => uc.UserId == userId);

            await foreach (var userContactRequest in userContactRequestResult)
            {
                userContactRequests.Add(userContactRequest.AsModel());
            }

            return userContactRequests;
        }

        public async Task<Data.Shared.Models.UserContact> AddUserContact(Data.Shared.Models.UserContactRequest request)
        {
            var userContactRequest = await _userContactRequestRepository.GetAsync(ucr => ucr.Id == request.Id);

            if (userContactRequest == null)
            {
                throw new NotFoundException();
            }

            var newContact = request.AsUserContact(DateTime.UtcNow).AsStore();
            var userContactResult = await _userContactRepository.AddOrUpdateAsync(uc => uc.UserId == request.UserId &&
                                                                                        uc.ContactUserId == request.ContactUserId,
                                                                                  newContact);
            if (userContactResult != null)
            {
                await _userContactRequestRepository.RemoveAsync(userContactRequest);

                return userContactResult.AsModel();
            }

            return null;
        }

        public async Task<Data.Shared.Models.UserContactRequest> AddUserContactRequest(Data.Shared.Models.UserContactRequest request)
        {
            var userContactResult = await _userContactRequestRepository.AddOrUpdateAsync(uc => uc.UserId == request.UserId &&
                                                                                               uc.ContactUserId == request.ContactUserId,
                                                                                         request.AsStore());

            return userContactResult.AsModel();
        }

        public async Task<bool> RemoveUserContactRequest(int id)
        {
            var userContactRequest = await _userContactRequestRepository.GetAsync(ucr => ucr.Id == id);

            if (userContactRequest == null)
            {
                throw new NotFoundException();
            }

            return await _userContactRequestRepository.RemoveAsync(userContactRequest);
        }

        public async Task<bool> RemoveUserContact(int id)
        {
            var userContactResult = await _userContactRepository.GetAsync(uc => uc.Id == id);

            if (userContactResult == null)
            {
                throw new NotFoundException();
            }

            return await _userContactRepository.RemoveAsync(userContactResult);
        }
    }
}
