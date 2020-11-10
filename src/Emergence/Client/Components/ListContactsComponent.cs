using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Client.Components
{
    public class ListContactsComponent : ListComponent<UserContact>
    {
        public override async Task<FindResult<UserContact>> GetListAsync(FindParams findParams)
        {
            var result = await ApiClient.FindContactsAsync(findParams);

            return new FindResult<UserContact>
            {
                Results = result.Results,
                Count = result.Count
            };
        }

        protected async Task RemoveContactAsync(UserContact userContact)
        {
            //TODO await ApiClient.RemoveContactAsync(userContact);
            //TODO refresh list
        }
    }
}
