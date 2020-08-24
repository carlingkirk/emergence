using System.Linq;
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

        protected async Task UpdateOriginAsync(Origin origin)
        {
            var result = await ModalServiceClient.ShowOriginModal(origin);
            if (!result.Cancelled)
            {
                origin = List.Where(o => o.OriginId == origin.OriginId).First();
                origin = result.Data as Origin;
            }
        }

        protected async Task UpdateParentOriginAsync(Origin origin)
        {
            var result = await ModalServiceClient.ShowOriginModal(origin);
            if (!result.Cancelled)
            {
                List.Where(o => o.ParentOrigin.OriginId == origin.OriginId).ToList().ForEach(o =>
                {
                    o.ParentOrigin = result.Data as Origin;
                });
            }
        }
    }
}
