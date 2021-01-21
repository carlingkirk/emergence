using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json.Serialization;
using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Search
{
    [TypeDiscriminator("Height")]
    public class HeightFilter : RangeFilter<double>
    {
        public HeightFilter(RangeFilter<double> filter)
        {
            Name = filter.Name;
            InputType = filter.InputType;
            FilterType = filter.FilterType;
            MinimumValue = filter.MinimumValue;
            MaximumValue = filter.MaximumValue;
        }

        public HeightFilter()
        {
            Name = "Height";
            InputType = InputType.SelectRange;
            FilterType = FilterType.Double;
            var values = new List<double> { 0, .5, 1, 2, 3, 5, 8, 10, 15, 30, 50, 100 };
            FacetValues = values.ToDictionary(m => m, c => (long?)0L);
        }
        [JsonIgnore]
        public Expression<Func<PlantInfo, bool>> Filter => p => (MinimumValue == 0 || (p.MinimumHeight != null && p.MinimumHeight >= MinimumValue)) &&
                                                                (MaximumValue == 0 || (p.MaximumHeight != null && p.MaximumHeight <= MaximumValue));
    }
}
