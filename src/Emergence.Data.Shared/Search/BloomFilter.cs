using System.Collections.Generic;
using System.Linq;
using Emergence.Data.Shared.Extensions;

namespace Emergence.Data.Shared.Search
{
    [TypeDiscriminator("Bloom")]
    public class BloomFilter : RangeFilter<int>, IFilterDisplay<int>
    {
        private IEnumerable<int> BloomValues { get; set; }
        public BloomFilter(RangeFilter<int> filter)
        {
            Name = filter.Name;
            InputType = filter.InputType;
            FilterType = filter.FilterType;
            MinimumValue = filter.MinimumValue;
            MaximumValue = filter.MaximumValue;
            BloomValues = GetBloomValues();
        }

        public BloomFilter()
        {
            Name = "Bloom";
            InputType = InputType.SelectRange;
            FilterType = FilterType.Integer;
            var values = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            FacetValues = values.ToDictionary(m => m, c => (long?)0L);
        }

        private IEnumerable<int> GetBloomValues()
        {
            var bloomValues = new List<int>();
            if (MinimumValue == 0 && MaximumValue == 0)
            {
                return bloomValues;
            }

            if (MinimumValue == 0)
            {
                MinimumValue = 1;
            }

            if (MaximumValue == 0)
            {
                MaximumValue = 12;
            }

            for (var i = MinimumValue; i <= MaximumValue; i++)
            {
                bloomValues.Add(i);
            }

            return bloomValues;
        }

        public string DisplayValue(int value, long? count = null)
        {
            var bloomValue = (Month)value;
            return bloomValue.ToFriendlyName();
        }
    }
}
