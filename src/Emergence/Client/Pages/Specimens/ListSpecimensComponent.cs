using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Client.Common;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Client.Components
{
    public class ListSpecimensComponent : ListComponent<Specimen>
    {
        protected static Dictionary<string, string> Headers =>
            new Dictionary<string, string>
            {
                { "ScientificName", "Scientific Name" },
                { "CommonName", "Common Name" },
                { "Quantity", "Quantity" },
                { "Stage", "Growth Stage" },
                { "Status", "Status" },
                { "DateAcquired", "Date Acquired" },
                { "Origin", "Origin" },
                { "", "" }
            };

        public override async Task<FindResult<Specimen>> GetListAsync(FindParams findParams)
        {
            var result = await ApiClient.FindSpecimensAsync(findParams);
            return new FindResult<Specimen>
            {
                Results = result.Results,
                Count = result.Count
            };
        }
    }
}
