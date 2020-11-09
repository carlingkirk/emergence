using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Extensions;
using Emergence.Data.Shared;
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

        public async Task<Data.Shared.Models.User> GetUserContactStatusAsync(Data.Shared.Models.User requestor, Data.Shared.Models.User user)
        {
            var userContact = await _userContactRepository.Where(uc => uc.UserId == user.Id && uc.ContactUserId == requestor.Id)
                .FirstOrDefaultAsync();

            if (userContact != null)
            {
                user.Contacts = new List<Data.Shared.Models.UserContact>
                {
                    userContact.AsModel()
                };
            }
            else
            {
                var userContactRequest = await _userContactRequestRepository.Where(uc => uc.UserId == user.Id && uc.ContactUserId == requestor.Id)
                    .FirstOrDefaultAsync();

                if (userContactRequest != null)
                {
                    user.ContactRequests = new List<Data.Shared.Models.UserContactRequest>
                    {
                        userContactRequest.AsModel()
                    };
                }
            }

            return user;
        }

        public async Task<IEnumerable<Data.Shared.Models.UserContact>> GetUserContactsAsync(int userId)
        {
            var userContacts = new List<Data.Shared.Models.UserContact>();
            var userContactResult = _userContactRepository.GetSomeAsync(uc => uc.UserId == userId);

            await foreach (var userContact in userContactResult)
            {
                userContacts.Add(userContact.AsModel());
            }

            return userContacts;
        }

        public async Task<IEnumerable<Data.Shared.Models.UserContactRequest>> GetUserContactRequestsAsync(int userId)
        {
            var userContactRequests = new List<Data.Shared.Models.UserContactRequest>();
            var userContactRequestResult = _userContactRequestRepository.GetSomeAsync(uc => uc.UserId == userId);

            await foreach (var userContactRequest in userContactRequestResult)
            {
                userContactRequests.Add(userContactRequest.AsModel());
            }

            return userContactRequests;
        }

        public async Task<Data.Shared.Models.UserContact> AddUserContactAsync(Data.Shared.Models.UserContactRequest request)
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

        public async Task<Data.Shared.Models.UserContactRequest> AddUserContactRequestAsync(Data.Shared.Models.UserContactRequest request)
        {
            var userContactResult = await _userContactRequestRepository.AddOrUpdateAsync(uc => uc.UserId == request.UserId &&
                                                                                               uc.ContactUserId == request.ContactUserId,
                                                                                         request.AsStore());

            return userContactResult.AsModel();
        }

        public async Task<bool> RemoveUserContactRequestAsync(int id)
        {
            var userContactRequest = await _userContactRequestRepository.GetAsync(ucr => ucr.Id == id);

            if (userContactRequest == null)
            {
                throw new NotFoundException();
            }

            return await _userContactRequestRepository.RemoveAsync(userContactRequest);
        }

        public async Task<bool> RemoveUserContactAsync(int id)
        {
            var userContactResult = await _userContactRepository.GetAsync(uc => uc.Id == id);

            if (userContactResult == null)
            {
                throw new NotFoundException();
            }

            return await _userContactRepository.RemoveAsync(userContactResult);
        }

        public async Task<FindResult<Data.Shared.Models.UserContact>> FindUserContacts(FindParams findParams, string userId)
        {
            var userContactQuery = _userContactRepository.WhereWithIncludes(uc => uc.User.UserId == userId &&
                                                                                  (findParams.SearchTextQuery == null ||
                                                                                   EF.Functions.Like(uc.ContactUser.DisplayName, findParams.SearchTextQuery)),
                                                                            false,
                                                                            uc => uc.Include(uc => uc.User),
                                                                            uc => uc.Include(uc => uc.ContactUser));

            userContactQuery = OrderBy(userContactQuery, findParams.SortBy, findParams.SortDirection);

            var count = userContactQuery.Count();
            var userContactResult = userContactQuery.GetSomeAsync(skip: findParams.Skip, take: findParams.Take, track: false);

            var userContacts = new List<Data.Shared.Models.UserContact>();
            await foreach (var userContact in userContactResult)
            {
                userContacts.Add(userContact.AsModel());
            }

            return new FindResult<Data.Shared.Models.UserContact>
            {
                Count = count,
                Results = userContacts
            };
        }

        public async Task<FindResult<Data.Shared.Models.UserContactRequest>> FindUserContactRequests(FindParams findParams, string userId)
        {
            var userContactQuery = _userContactRequestRepository.WhereWithIncludes(uc => uc.User.UserId == userId &&
                                                                                  (findParams.SearchTextQuery == null ||
                                                                                   EF.Functions.Like(uc.ContactUser.DisplayName, findParams.SearchTextQuery)),
                                                                                   false,
                                                                                   uc => uc.Include(uc => uc.User).Include(uc => uc.ContactUser));

            userContactQuery = OrderBy(userContactQuery, findParams.SortBy, findParams.SortDirection);

            var count = userContactQuery.Count();
            var userContactResult = userContactQuery.GetSomeAsync(skip: findParams.Skip, take: findParams.Take, track: false);

            var userContacts = new List<Data.Shared.Models.UserContactRequest>();
            await foreach (var userContact in userContactResult)
            {
                userContacts.Add(userContact.AsModel());
            }

            return new FindResult<Data.Shared.Models.UserContactRequest>
            {
                Count = count,
                Results = userContacts
            };
        }

        private IQueryable<UserContact> OrderBy(IQueryable<UserContact> userContactQuery, string sortBy = null, SortDirection sortDirection = SortDirection.None)
        {
            if (sortDirection == SortDirection.None)
            {
                return userContactQuery;
            }
            if (sortBy == null)
            {
                sortBy = "DisplayName";
            }
            var userContactSorts = new Dictionary<string, Expression<Func<UserContact, object>>>
            {
                { "DisplayName", uc => uc.ContactUser.DisplayName },
                { "DateAccepted", uc => uc.DateAccepted }
            };

            if (sortDirection == SortDirection.Descending)
            {
                userContactQuery = userContactQuery.WithOrder(a => a.OrderByDescending(userContactSorts[sortBy]));
            }
            else
            {
                userContactQuery = userContactQuery.WithOrder(a => a.OrderBy(userContactSorts[sortBy]));
            }

            return userContactQuery;
        }

        private IQueryable<UserContactRequest> OrderBy(IQueryable<UserContactRequest> userContactQuery, string sortBy = null, SortDirection sortDirection = SortDirection.None)
        {
            if (sortDirection == SortDirection.None)
            {
                return userContactQuery;
            }
            if (sortBy == null)
            {
                sortBy = "DisplayName";
            }
            var userContactSorts = new Dictionary<string, Expression<Func<UserContactRequest, object>>>
            {
                { "DisplayName", uc => uc.ContactUser.DisplayName },
                { "DateRequested", uc => uc.DateRequested }
            };

            if (sortDirection == SortDirection.Descending)
            {
                userContactQuery = userContactQuery.WithOrder(a => a.OrderByDescending(userContactSorts[sortBy]));
            }
            else
            {
                userContactQuery = userContactQuery.WithOrder(a => a.OrderBy(userContactSorts[sortBy]));
            }

            return userContactQuery;
        }
    }
}
