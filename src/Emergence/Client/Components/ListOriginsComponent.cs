using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Client.Components
{
    public class ListOriginsComponent : ListComponent<Origin>
    {
        public override async Task<FindResult<Origin>> GetListAsync(FindParams findParams)
        {
            var result = await ApiClient.FindOriginsAsync(findParams);

            return new FindResult<Origin>
            {
                Results = result.Results,
                Count = result.Count
            };
        }
    }
}
