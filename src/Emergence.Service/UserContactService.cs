using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;
using Emergence.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Emergence.Service
{
    public class UserContactService : IUserContactService
    {
        private readonly IRepository<UserContact> _userContactRepository;
        public UserContactService(IRepository<UserContact> userContactRepository)
        {
            _userContactRepository = userContactRepository;
        }

        public async Task<Data.Shared.Models.UserContact> GetUserContact(Data.Shared.Models.User requestor, User user)
        {
            var userContact = await _userContactRepository.Where(uc => uc.UserId == user.Id && uc.ContactUserId == requestor.Id)
                .FirstOrDefaultAsync();

            return userContact.AsModel();
        }
    }
}
