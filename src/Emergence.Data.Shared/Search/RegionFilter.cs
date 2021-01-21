using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Emergence.Data.Shared.Stores;

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
            Values = new List<string>
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
            };
        }

        public Expression<Func<PlantInfo, bool>> Filter => p => p.PlantLocations != null && p.PlantLocations.Any(pl => pl.Location.Region == Value);

        public string DisplayValue(string value, long? count = null)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            else
            {
                return $"{value} ({count})";
            }
        }
    }
}
