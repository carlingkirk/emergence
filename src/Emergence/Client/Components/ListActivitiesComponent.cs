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

        protected override async Task OnInitializedAsync() => Activities = await FindActivitiesAsync();

        protected async Task<IEnumerable<Activity>> FindActivitiesAsync() => await ApiClient.FindActivitiesAsync(SearchText);
    }
}
