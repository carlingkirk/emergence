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
    }

    public class SelectFilter<TValue> : Filter<TValue>
    {
        public Dictionary<TValue, long?> FacetValues { get; set; }
    }

    public class SelectRangeFilter<TValue> : RangeFilter<TValue>
    {
        public Dictionary<TValue, long?> MinFacetValues { get; set; }
        public Dictionary<TValue, long?> MaxFacetValues { get; set; }
    }

    public class RangeFilter<TValue> : SelectFilter<TValue>
    {
        public TValue MinimumValue { get; set; }
        public TValue MaximumValue { get; set; }
    }
}
