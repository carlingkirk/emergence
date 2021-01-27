using System.Threading.Tasks;
using Emergence.Client.Common;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Client.Components
{
    public class ListLifeformsComponent : ListComponent<Lifeform>
    {
        public override async Task<FindResult<Lifeform>> GetListAsync(FindParams findParams)
        {
            var lifeformFindParams = new FindParams<Lifeform>
            {
                CreatedBy = findParams.CreatedBy,
                SearchText = findParams.SearchText,
                Skip = findParams.Skip,
                Take = findParams.Take
            };
            var result = await ApiClient.FindLifeformsAsync(lifeformFindParams);

            return new FindResult<Lifeform>
            {
                Results = result.Results,
                Count = result.Count
            };
        }
    }
}
