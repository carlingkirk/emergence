using System.Collections.Generic;
using System.Linq;

namespace Emergence.Data.Shared.Search
{
    [TypeDiscriminator("Spread")]
    public class SpreadFilter : SelectRangeFilter<double>, IFilterDisplay<double>
    {
        public IEnumerable<double> Values { get; set; }

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
            MinFacetValues = Values.ToDictionary(m => m, c => (long?)0L);
            MaxFacetValues = Values.ToDictionary(m => m, c => (long?)0L);
        }

        public string DisplayValue(double value, long? count = null)
        {
            if (value == 0)
            {
                return value.ToString();
            }
            else
            {
                return count != null ? $"{value} ({count})" : $"{value}";
            }
        }
    }
}
