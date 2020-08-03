using System.Linq;
using System.Threading.Tasks;
using Emergence.Client.Common;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class ListActivitiesComponent : ListComponent<Activity>
    {
        [Inject]
        protected IApiClient ApiClient { get; set; }
        [Inject]
        protected IModalServiceClient ModalServiceClient { get; set; }

        public override async Task<FindResult<Activity>> GetListAsync(string searchText, int? skip = 0, int? take = 10, string sortBy = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            var result = await ApiClient.FindActivitiesAsync(SearchText, skip, Take, SortBy, SortDirection);
            return result;
        }

        protected async Task UpdateActivityAsync(Activity activity)
        {
            var result = await ModalServiceClient.ShowActivityModal(activity);
            activity = List.Where(a => a.ActivityId == activity.ActivityId).First();
            activity = result.Data as Activity;
        }
    }
}
