using System.Collections.Generic;
using System.Linq;
using Emergence.Data.Shared.Extensions;

namespace Emergence.Data.Shared.Search
{
    [TypeDiscriminator("Bloom")]
    public class BloomFilter : SelectRangeFilter<string, Month>, IFilterDisplay<string>
    {
        public BloomFilter(SelectRangeFilter<string> filter)
        {
            Name = filter.Name;
            InputType = filter.InputType;
            FilterType = filter.FilterType;
            MinimumValue = filter.MinimumValue;
            MaximumValue = filter.MaximumValue;
        }

        public BloomFilter()
        {
            Name = "Bloom";
            InputType = InputType.SelectRange;
            FilterType = FilterType.Integer;
            var values = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            FacetValues = values.ToDictionary(m => m.ToString(), c => (long?)0L);
        }

        public string DisplayValue(string value, long? count = null)
        {
            if (int.TryParse(value, out var bloomValue))
            {
                var bloomMonth = (Month)bloomValue;
                return count != null ? $"{bloomMonth.ToFriendlyName()} ({count})" : $"{bloomMonth.ToFriendlyName()}";
            }
            else
            {
                return value.ToString();
            }
        }

        public override Dictionary<string, long?> GetFacetValues(Dictionary<string, long?> values)
        {
            values = values.ToDictionary(k => int.Parse(k.Key), v => v.Value).OrderBy(k => k.Key).ToDictionary(k => k.Key.ToString(), v => v.Value);
            return values.GetFacetValues<Month>(null);
        }
    }
}
