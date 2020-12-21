using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Emergence.Data.Shared.Search
{
    public interface IFilter<T>
    {
        public string Name { get; }
        public InputType InputType { get; }
        public FilterType FilterType { get; }
        public Expression<Func<T, bool>> Filter { get; }
        public bool IsFiltered { get; }
    }

    public interface IBooleanFilter<T> : IFilter<T>
    {
        public bool Value { get; set; }
    }

    public interface ISelectFilter<T, TValue> : IFilter<T>
    {
        public TValue Value { get; set; }
        IEnumerable<TValue> Values { get; }
    }

    public interface IRangeFilter<T, TValue> : IFilter<T>
    {
        public TValue MinimumValue { get; set; }
        public TValue MaximumValue { get; set; }
    }
    public interface ISelectRangeFilter<T, TValue> : IFilter<T>
    {
        IEnumerable<TValue> Values { get; }
        public TValue MinimumValue { get; set; }
        public TValue MaximumValue { get; set; }
    }

    public enum InputType
    {
        Boolean,
        Select,
        SelectRange,
        Range
    }

    public enum FilterType
    {
        String,
        Integer,
        Double
    }
}
