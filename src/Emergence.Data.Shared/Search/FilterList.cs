using System.Collections.Generic;

namespace Emergence.Data.Shared.Search
{
    public static class FilterList
    {
        public static List<Filter> GetPlantInfoFilters() => new List<Filter>
        {
            new RegionFilter()
        };

        public static List<Filter> GetSpecimenFilters() => new List<Filter>
        {
            new StageFilter()
        };
    }
}
