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

        public override Dictionary<double, long?> GetFacetValues(Dictionary<string, long?> values)
        {
            var facetValues = values.ToDictionary(k => double.Parse(k.Key), v => v.Value).OrderBy(k => k.Key).ToDictionary(k => k.Key, v => v.Value);
            return facetValues;
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
