using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Client.Components
{
    public class ListSpecimensComponent : ListComponent<Specimen>
    {
        public override async Task<FindResult<Specimen>> GetListAsync(FindParams findParams)
        {
            var result = await ApiClient.FindSpecimensAsync(findParams);
            return new FindResult<Specimen>
            {
                Results = result.Results,
                Count = result.Count
            };
        }
    }
}
