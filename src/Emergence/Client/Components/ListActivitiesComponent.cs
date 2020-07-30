using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Client.Common;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class ListActivitiesComponent : ComponentBase
    {
        [Inject]
        protected IApiClient ApiClient { get; set; }
        public IEnumerable<Activity> Activities { get; set; }
        public string SearchText { get; set; }
        public string SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Skip = 0;
            Take = 10;
            await FindActivitiesAsync();
        }

        protected async Task FindActivitiesAsync() => Activities = await ApiClient.FindActivitiesAsync(SearchText, Skip, Take, SortBy, SortDirection);

        protected async Task SortActivitiesAsync(string sortBy)
        {
            if (SortDirection == SortDirection.Descending)
            {
                SortDirection = SortDirection.Ascending;
            }
            else
            {
                SortDirection = SortDirection.Descending;
            }

            SortBy = sortBy;
            await FindActivitiesAsync();
        }

        protected string GetSortClass(string name)
        {
            if (SortBy == name)
            {
                if (SortDirection == SortDirection.Descending)
                {
                    return "oi oi-caret-bottom";
                }
                else
                {
                    return "oi oi-caret-top";
                }
            }
            return "";
        }
    }
}
