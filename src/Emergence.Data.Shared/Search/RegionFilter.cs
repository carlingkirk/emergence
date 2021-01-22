using System.Collections.Generic;
using System.Linq;

namespace Emergence.Data.Shared.Search
{
    [TypeDiscriminator("Region")]
    public class RegionFilter : SelectFilter<string>, IFilterDisplay<string>
    {
        public RegionFilter(Filter<string> filter)
        {
            Name = filter.Name;
            InputType = filter.InputType;
            FilterType = filter.FilterType;
            Value = filter.Value;
        }

        public RegionFilter()
        {
            Name = "Region";
            InputType = InputType.Select;
            FilterType = FilterType.String;
            FacetValues = new List<string>
            {
                "",
                "Africa",
                "Antarctica/Southern Ocean",
                "Australia",
                "Caribbean",
                "Eastern Atlantic Ocean",
                "East Pacific",
                "Europe & Northern Asia (excluding China)",
                "Indo-West Pacific",
                "Oceania",
                "North America",
                "Middle America",
                "South America",
                "Southern Asia",
                "Western Atlantic Ocean"
            }.ToDictionary(m => m, c => (long?)0L);
        }

        public string DisplayValue(string value, long? count = null)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            else
            {
                return count != null ? $"{value} ({count})" : $"{value}";
            }
        }
    }
}
