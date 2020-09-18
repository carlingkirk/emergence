using System.Linq;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Client.Components
{
    public class ListLifeformsComponent : ListComponent<Lifeform>
    {
        public override async Task<FindResult<Lifeform>> GetListAsync(FindParams findParams)
        {
            var result = await ApiClient.FindLifeformsAsync(findParams);

            return new FindResult<Lifeform>
            {
                Results = result.Results,
                Count = result.Count
            };
        }
    }
}
