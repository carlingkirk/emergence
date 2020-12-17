using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Client.Common;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;
using Emergence.Data.Shared.Search;

namespace Emergence.Client.Components
{
    public class ListPlantInfosComponent : ListComponent<PlantInfo>
    {
        protected static Dictionary<string, string> Headers =>
            new Dictionary<string, string>
            {
                { "ScientificName", "Scientific Name" },
                { "CommonName", "Common Name" },
                { "Origin", "Origin" },
                { "Zone", "Zone" },
                { "Light", "Light" },
                { "Water", "Water" },
                { "BloomTime", "Bloom Time" },
                { "Height", "Height" },
                { "Spread", "Spread" }
            };

        protected override async Task OnInitializedAsync()
        {
            Filters = FilterList.GetPlantInfoFilters();

            await base.OnInitializedAsync();
        }

        public override async Task<FindResult<PlantInfo>> GetListAsync(FindParams findParams)
        {
            findParams.Filters = Filters;

            var result = await ApiClient.FindPlantInfosAsync(findParams);

            return new FindResult<PlantInfo>
            {
                Results = result.Results,
                Count = result.Count
            };
        }
    }
}
