using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Client.Common;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;
using Emergence.Data.Shared.Search;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class ListPlantInfosComponent : ListComponent<PlantInfo>
    {
        [Parameter]
        public Lifeform Lifeform { get; set; }
        public PlantInfoFilters PlantInfoFilters { get; set; }

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

        public override async Task<FindResult<PlantInfo>> GetListAsync(FindParams findParams)
        {
            var findPlantInfoParams = new PlantInfoFindParams
            {
                SearchText = findParams.SearchText,
                UseNGrams = false,
                Skip = findParams.Skip,
                Take = findParams.Take,
                SortBy = findParams.SortBy,
                SortDirection = findParams.SortDirection,
                Filters = PlantInfoFilters,
                CreatedBy = findParams.CreatedBy
            };

            if (Lifeform != null)
            {
                findPlantInfoParams.Lifeform = Lifeform;
            }

            var result = await ApiClient.FindPlantInfosAsync(findPlantInfoParams);

            PlantInfoFilters = result.Filters;

            return new PlantInfoFindResult
            {
                Results = result.Results,
                Count = result.Count
            };
        }
    }
}
