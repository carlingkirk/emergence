using System.Threading.Tasks;
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
            await FindAsync();
        }
    }
}
