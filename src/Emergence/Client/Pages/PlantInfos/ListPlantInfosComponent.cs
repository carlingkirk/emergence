using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Client.Common;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;
using Emergence.Data.Shared.Search;
using Stores = Emergence.Data.Shared.Stores;

namespace Emergence.Client.Components
{
    public class ListPlantInfosComponent : ListComponent<PlantInfo>
    {
        public IEnumerable<IFilter<Stores.PlantInfo>> Filters { get; set; }
        public int FilterCount => Filters != null ? Filters.Count(f => f.IsFiltered) : 0;
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

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Filters = new List<IFilter<Stores.PlantInfo>>
            {
                new RegionFilter()
            };
        }

        public override async Task<FindResult<PlantInfo>> GetListAsync(FindParams findParams)
        {
            var result = await ApiClient.FindPlantInfosAsync(findParams);

            return new FindResult<PlantInfo>
            {
                Results = result.Results,
                Count = result.Count
            };
        }
    }
}
