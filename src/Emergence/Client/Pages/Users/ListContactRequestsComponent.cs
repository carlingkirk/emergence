using System.Threading.Tasks;
using Emergence.Client.Common;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Client.Components
{
    public class ListContactRequestsComponent : ListComponent<UserContactRequest>
    {
        public override async Task<FindResult<UserContactRequest>> GetListAsync(FindParams findParams)
        {
            var result = await ApiClient.FindContactRequestsAsync(findParams);

            return new FindResult<UserContactRequest>
            {
                Results = result.Results,
                Count = result.Count
            };
        }

        protected async Task AddContactAsync(UserContactRequest userContactRequest)
        {
            var userContact = await ApiClient.AddContactAsync(userContactRequest);
            if (userContact != null)
            {
                await FindAsync();
            }
        }

        protected async Task RemoveContactRequestAsync(UserContactRequest userContactRequest)
        {
            var result = await ApiClient.RemoveContactRequestAsync(userContactRequest);
            if (result)
            {
                await FindAsync();
            }
        }
    }
}
