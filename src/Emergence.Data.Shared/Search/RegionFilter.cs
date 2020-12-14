using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Search
{
    public class RegionFilter : SelectFilter<string>
    {
        public RegionFilter()
        {
            Name = "Location";
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

        public Expression<Func<PlantInfo, bool>> Filter => p => p.PlantLocations.Any(pl => pl.Location.Region == Value);
    }
}
