using System.Collections.Generic;

namespace Emergence.Data.Shared.Search
{
    public class Filter
    {
        public string Name { get; set; }
        public InputType InputType { get; set; }
        public FilterType FilterType { get; set; }
    }

    public class Filter<TValue> : Filter
    {
        public TValue Value { get; set; }
        public virtual bool IsFiltered() => Value switch
        {
            string stringValue => !string.IsNullOrEmpty(stringValue),
            int intValue => intValue != 0,
            _ => false,
        };
    }

    public class SelectFilter<TValue> : Filter<TValue>
    {
        public IEnumerable<TValue> Values { get; set; }
        public override bool IsFiltered() => Values switch
        {
            string stringValue => !string.IsNullOrEmpty(stringValue),
            int intValue => intValue != 0,
            _ => false,
        };
    }

    public class RangeFilter<TValue> : SelectFilter<TValue>
    {
        public TValue MinimumValue { get; set; }
        public TValue MaximumValue { get; set; }

        public override bool IsFiltered()
        {
            var minValue = MinimumValue switch
            {
                string stringValue => !string.IsNullOrEmpty(stringValue),
                int intValue => intValue != 0,
                _ => false,
            };

            var maxValue = MaximumValue switch
            {
                string stringValue => !string.IsNullOrEmpty(stringValue),
                int intValue => intValue != 0,
                _ => false,
            };

            return minValue || maxValue;
        }
    }
}
