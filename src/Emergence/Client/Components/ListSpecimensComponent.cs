using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Client.Common;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class ListSpecimensComponent : ComponentBase
    {
        [Inject]
        protected IApiClient ApiClient { get; set; }
        public IEnumerable<Specimen> Specimens { get; set; }
        public string SearchText { get; set; }

        protected override async Task OnInitializedAsync() => await GetSpecimensAsync();

        protected async Task GetSpecimensAsync() => Specimens = await ApiClient.FindSpecimensAsync(SearchText);
    }
}
