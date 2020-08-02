using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Client.Common;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class ListActivitiesComponent : ComponentBase
    {
        [Inject]
        protected IApiClient ApiClient { get; set; }
        [Inject]
        protected IModalServiceClient ModalServiceClient { get; set; }
        public IEnumerable<Activity> Activities { get; set; }
        public string SearchText { get; set; }
        public string SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
        public int CurrentPage { get; set; }
        public int Take { get; set; }
        public int Count { get; set; }

        protected override async Task OnInitializedAsync()
        {
            CurrentPage = 1;
            Take = 10;
            await FindActivitiesAsync();
        }

        protected async Task FindActivitiesAsync()
        {
            var skip = (CurrentPage - 1) * Take;
            var result = await ApiClient.FindActivitiesAsync(SearchText, skip, Take, SortBy, SortDirection);
            Activities = result.Results;
            Count = result.Count;
        }

        protected async Task<IEnumerable<Activity>> GetSortActivitiesAsync(string sortBy, SortDirection sortDirection)
        {
            SortDirection = sortDirection;
            SortBy = sortBy;
            await FindActivitiesAsync();
            return Activities;
        }

        protected async Task<IEnumerable<Activity>> PageAsync(int page)
        {
            CurrentPage = page;
            await FindActivitiesAsync();
            return Activities;
        }

        protected async Task UpdateActivity(Activity activity)
        {
            var result = await ModalServiceClient.ShowActivityModal(activity);
            activity = Activities.Where(a => a.ActivityId == activity.ActivityId).First();
            activity = result.Data as Activity;
        }
    }
}
