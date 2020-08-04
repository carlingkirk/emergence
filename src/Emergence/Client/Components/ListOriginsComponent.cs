using System.Linq;
using System.Threading.Tasks;
using Emergence.Client.Common;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class ListOriginsComponent : ListComponent<Origin>
    {
        [Inject]
        protected IApiClient ApiClient { get; set; }
        [Inject]
        protected IModalServiceClient ModalServiceClient { get; set; }

        public override async Task<FindResult<Origin>> GetListAsync(string searchText, int? skip = 0, int? take = 10, string sortBy = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            var result = await ApiClient.FindOriginsAsync(SearchText, skip, Take, SortBy, SortDirection);
            return new FindResult<Origin>
            {
                Results = result.Results,
                Count = result.Count
            };
        }

        protected async Task UpdateOriginAsync(Origin origin)
        {
            var result = await ModalServiceClient.ShowOriginModal(origin);
            origin = List.Where(o => o.OriginId == origin.OriginId).First();
            origin = result.Data as Origin;
        }
    }
}
