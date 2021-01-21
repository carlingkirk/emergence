using System.Collections.Generic;
using Emergence.Data.Shared.Models;
using Emergence.Data.Shared.Search;

namespace Emergence.Data.Shared
{
    public class FindResult<T> where T : class
    {
        public long Count { get; set; }
        public IEnumerable<T> Results { get; set; }
    }

    public class PlantInfoFindResult : FindResult<PlantInfo>
    {
        public PlantInfoFilters Filters { get; set; }
    }
}
