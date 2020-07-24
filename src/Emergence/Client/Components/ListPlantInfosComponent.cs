using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Client.Common;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class ListPlantInfosComponent : ComponentBase
    {
        [Inject]
        protected IApiClient ApiClient { get; set; }
        public IEnumerable<PlantInfo> PlantInfos { get; set; }
        public string SearchText { get; set; }

        protected override async Task OnInitializedAsync() => await GetPlantInfosAsync();

        protected async Task GetPlantInfosAsync() => PlantInfos = await ApiClient.FindPlantInfosAsync(SearchText);
    }
}
