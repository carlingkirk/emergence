using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Client.Common;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class ListOriginsComponent : ComponentBase
    {
        [Inject]
        protected IApiClient ApiClient { get; set; }
        public IEnumerable<Origin> Origins { get; set; }
        public string SearchText { get; set; }

        protected override async Task OnInitializedAsync() => await GetOriginsAsync();

        protected async Task GetOriginsAsync() => Origins = await ApiClient.FindOriginsAsync(SearchText);
    }
}
