using System.Collections.Generic;
using System.Linq;

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
            var values = new List<double> { 0, .5, 1, 2, 3, 5, 8, 10, 15, 30, 50, 100 };
            FacetValues = values.ToDictionary(m => m, c => (long?)0L);
        }
    }
}
