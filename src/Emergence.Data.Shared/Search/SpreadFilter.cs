using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Search
{
    [TypeDiscriminator("Spread")]
    public class SpreadFilter : RangeFilter<double>
    {
        public SpreadFilter(RangeFilter<double> filter)
        {
            Name = filter.Name;
            InputType = filter.InputType;
            FilterType = filter.FilterType;
            MinimumValue = filter.MinimumValue;
            MaximumValue = filter.MaximumValue;
        }

        public SpreadFilter()
        {
            Name = "Spread";
            InputType = InputType.SelectRange;
            FilterType = FilterType.Double;
            Values = new List<double> { 0, .5, 1, 2, 3, 5, 8, 10, 15, 30, 50, 100 };
        }

        public Expression<Func<PlantInfo, bool>> Filter => p => (MinimumValue == 0 || (p.MinimumSpread != null && p.MinimumSpread >= MinimumValue)) &&
                                                                (MaximumValue == 0 || p.MaximumSpread <= MaximumValue);
    }
}
