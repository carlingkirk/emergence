using System;
using System.Collections.Generic;
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
        public IEnumerable<Activity> Activities { get; set; }
        public string SearchText { get; set; }
        public string SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling(Count / (double)Take);
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

        protected async Task PageAsync(int pages)
        {
            CurrentPage += pages;
            await FindActivitiesAsync();
        }
    }
}
