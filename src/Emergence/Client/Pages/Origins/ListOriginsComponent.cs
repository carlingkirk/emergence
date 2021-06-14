using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Client.Common;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Client.Components
{
    public class ListOriginsComponent : ListComponent<Origin>
    {
        protected static Dictionary<string, string> Headers =>
            new Dictionary<string, string>
            {
                { "Name", "Name" },
                { "Type", "Type" },
                { "Description", "Description" },
                { "ParentOrigin", "Parent Origin" },
                { "City", "City" },
                { "Link", "Link" }
            };
        public override async Task<FindResult<Origin>> GetListAsync(FindParams findParams)
        {
            var result = await ApiClient.FindOriginsAsync(findParams);

            return new FindResult<Origin>
            {
                Results = result.Results,
                Count = result.Count
            };
        }
    }
}
