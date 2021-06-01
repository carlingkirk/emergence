using System;
using System.Collections.Generic;
using Emergence.Data.Shared.Extensions;

namespace Emergence.Data.Shared.Search
{
    public class Filter
    {
        public string Name { get; set; }
        public InputType InputType { get; set; }
        public FilterType FilterType { get; set; }
    }

    public class Filter<TValue> : Filter where TValue : IComparable
    {
        public TValue Value { get; set; }
    }

    public abstract class SelectFilter<TValue> : Filter<TValue> where TValue : IComparable
    {
        public Dictionary<TValue, long?> FacetValues { get; set; }

        public abstract Dictionary<TValue, long?> GetFacetValues(Dictionary<string, long?> values);
    }

    public abstract class SelectRangeFilter<TValue> : RangeFilter<TValue> where TValue : IComparable
    {
        public Dictionary<TValue, long?> MinFacetValues { get; set; }
        public Dictionary<TValue, long?> MaxFacetValues { get; set; }
    }

    public abstract class SelectRangeFilter<TValue, TEnum> : SelectRangeFilter<TValue> where TValue : IComparable where TEnum : struct, Enum
    {
        public string GetFacetValue(string value) => ((int)(object)value.GetEnumValue<TEnum>()).ToString();
    }

    public abstract class RangeFilter<TValue> : SelectFilter<TValue> where TValue : IComparable
    {
        public TValue MinimumValue { get; set; }
        public TValue MaximumValue { get; set; }
    }

    public abstract class RangeFilter<TValue, TEnum> : RangeFilter<TValue> where TValue : IComparable where TEnum : struct, Enum
    {
        public string GetFacetValue(string value) => ((int)(object)value.GetEnumValue<TEnum>()).ToString();
    }
}
