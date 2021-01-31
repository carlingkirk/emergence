using System.Collections.Generic;
using System.Linq;

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

        public string DisplayValue(double value, long? count = null)
        {
            if (value > 0)
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
