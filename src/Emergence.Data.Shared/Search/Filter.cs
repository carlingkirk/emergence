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
        public IDictionary<TValue, long?> FacetValues { get; set; }
        public override bool IsFiltered() => FacetValues != null && FacetValues switch
        {
            string stringValue => !string.IsNullOrEmpty(stringValue),
            int intValue => intValue != 0,
            double doubleValue => doubleValue != 0,
            _ => false
        };
    }

    public class SelectRangeFilter<TValue> : RangeFilter<TValue>
    {
        public IDictionary<TValue, long?> MinFacetValues { get; set; }
        public IDictionary<TValue, long?> MaxFacetValues { get; set; }
        public override bool IsFiltered() => MinFacetValues != null && MinFacetValues switch
        {
            string stringValue => !string.IsNullOrEmpty(stringValue),
            int intValue => intValue != 0,
            double doubleValue => doubleValue != 0,
            _ => false
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
