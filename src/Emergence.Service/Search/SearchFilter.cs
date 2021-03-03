using System;
using System.Collections.Generic;
using Nest;

namespace Emergence.Service.Search
{
    public class SearchFilter<T> where T : class
    {
        public string Name { get; set; }
        public string Field { get; set; }

        public SearchFilter(string name, string field)
        {
            Name = name;
            Field = field;
        }

        public virtual AggregationContainerDescriptor<T> ToAggregationContainerDescriptor(AggregationContainerDescriptor<T> aggregationDescriptor) =>
            aggregationDescriptor
                .Terms(Name, t => t
                    .Field(Field));
    }

    public class SearchRangeFilter<T, TValue> : SearchFilter<T> where T : class
    {
        public string MinField { get; set; }
        public string MaxField { get; set; }
        public IEnumerable<TValue> Values { get; set; }
        public string Value { get; set; }
        public TValue MinValue { get; set; }
        public TValue MaxValue { get; set; }

        public SearchRangeFilter(string name, string minField, string maxField, IEnumerable<TValue> values, TValue minValue, TValue maxValue, string field = null) : base(name, field)
        {
            Name = name;
            MinField = minField;
            MaxField = maxField;
            Values = values;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public QueryContainer ToFilter(QueryContainerDescriptor<T> queryContainerDescriptor)
        {
            QueryContainer query = null;

            if ((MinValue != null && !string.IsNullOrEmpty(MinValue?.ToString())) || (MaxValue != null && !string.IsNullOrEmpty(MaxValue.ToString())))
            {
                var hasMin = double.TryParse(MinValue?.ToString(), out var minValue) && minValue > 0;
                var hasMax = double.TryParse(MaxValue?.ToString(), out var maxValue) && maxValue > 0;

                if (hasMin && minValue > 0 && hasMax && maxValue > 0)
                {
                    return queryContainerDescriptor.Bool(b => b.Must(m => (m.Exists(e => e.Field(MinField)) || m.Exists(e => e.Field(MaxField))) &&
                                                                        (m.Range(r => r.Field(MinField).GreaterThanOrEquals(minValue)) ||
                                                                        (!m.Exists(e => e.Field(MinField)) &&
                                                                        (m.Range(r => r.Field(MaxField).LessThanOrEquals(maxValue)) ||
                                                                         !m.Exists(e => e.Field(MaxField)))))));
                }
                else
                {
                    if (hasMin && minValue > 0)
                    {
                        return queryContainerDescriptor.Bool(b => b.Must(m => m.Range(r => r.Field(MinField).GreaterThanOrEquals(minValue))));
                    }
                    else if (hasMax && maxValue > 0)
                    {
                        return queryContainerDescriptor.Bool(b => b.Must(m => m.Range(r => r.Field(MaxField).LessThanOrEquals(maxValue))));
                    }
                }
            }

            return query;
        }

        public override AggregationContainerDescriptor<T> ToAggregationContainerDescriptor(AggregationContainerDescriptor<T> aggregationDescriptor)
        {
            var fromRanges = new List<Func<AggregationRangeDescriptor, IAggregationRange>>();
            var toRanges = new List<Func<AggregationRangeDescriptor, IAggregationRange>>();
            foreach (var valueOption in Values)
            {
                var value = double.Parse(valueOption.ToString());
                fromRanges.Add(r =>
                {
                    r.From(value)
                    .To(9999)
                     .Key(value.ToString());

                    return r;
                });
                toRanges.Add(r =>
                {
                    r.From(0)
                     .To(value)
                     .Key(value.ToString());

                    return r;
                });
            }

            aggregationDescriptor.Range("Min" + Name, r => r.Field(MinField).Ranges(fromRanges.ToArray()));
            aggregationDescriptor.Range("Max" + Name, r => r.Field(MaxField).Ranges(toRanges.ToArray()));

            return aggregationDescriptor;
        }
    }

    public class SearchValueFilter<T, TValue> : SearchFilter<T> where T : class
    {
        public TValue Value { get; set; }

        public SearchValueFilter(string name, string field, TValue value) : base(name, field)
        {
            Value = value;
        }

        public QueryContainer ToFilter(QueryContainerDescriptor<T> queryContainerDescriptor)
        {
            int.TryParse(Value?.ToString(), out var value);

            QueryContainer query = null;

            if (value > 0)
            {
                return queryContainerDescriptor.Term(Field, value);
            }

            return query;
        }

        public override AggregationContainerDescriptor<T> ToAggregationContainerDescriptor(AggregationContainerDescriptor<T> aggregationDescriptor) =>
            aggregationDescriptor
                 .Terms(Name, t => t
                     .Field(Field));
    }

    public class SearchValuesFilter<T, TValue> : SearchFilter<T> where T : class
    {
        public TValue MinValue { get; set; }
        public TValue MaxValue { get; set; }

        public SearchValuesFilter(string name, string field, TValue minValue, TValue maxValue) : base(name, field)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public QueryContainer ToFilter(QueryContainerDescriptor<T> queryContainerDescriptor)
        {
            int.TryParse(MinValue?.ToString(), out var minValue);
            int.TryParse(MaxValue?.ToString(), out var maxValue);

            QueryContainer query = null;
            if (minValue > 0 && maxValue > 0)
            {
                var values = new List<int>();
                for (var i = minValue; i <= maxValue; i++)
                {
                    values.Add(i);
                }
                return queryContainerDescriptor.Terms(t => t.Field(Field).Terms(values));
            }
            else
            {
                if (minValue > 0)
                {
                    return queryContainerDescriptor.Term(Field, minValue);
                }
                else if (maxValue > 0)
                {
                    return queryContainerDescriptor.Term(Field, maxValue);
                }
            }

            return query;
        }

        public override AggregationContainerDescriptor<T> ToAggregationContainerDescriptor(AggregationContainerDescriptor<T> aggregationDescriptor)
        {
            if ((MinValue != null && !string.IsNullOrEmpty(MinValue?.ToString())) || (MaxValue != null && !string.IsNullOrEmpty(MaxValue.ToString())))
            {
                int.TryParse(MinValue?.ToString(), out var minValue);
                int.TryParse(MaxValue?.ToString(), out var maxValue);

                return aggregationDescriptor
                    .Filter(Name, f => f.Filter(f =>
                    {
                        QueryContainer query = null;
                        if (minValue > 0 && maxValue > 0)
                        {
                            var values = new List<int>();
                            for (var i = minValue; i <= maxValue; i++)
                            {
                                values.Add(i);
                            }
                            query = f.Terms(t => t.Field(Field).Terms(values));
                        }
                        else
                        {
                            if (minValue > 0)
                            {
                                query = f.Term(Field, minValue);
                            }
                            else if (maxValue > 0)
                            {
                                query = f.Term(Field, maxValue);
                            }
                        }

                        return query;
                    }))
                 .Terms(Name, t => t
                     .Field(Field));
            }
            else
            {
                return aggregationDescriptor
                 .Terms(Name, t => t
                     .Field(Field));
            }
        }
    }

    public class NestedSearchFilter<T> : SearchFilter<T> where T : class
    {
        public string Path { get; set; }

        public NestedSearchFilter(string name, string field, string path) : base(name, field)
        {
            Path = path;
        }
    }

    public class NestedSearchValueFilter<T, TValue> : NestedSearchFilter<T> where T : class
    {
        public NestedSearchValueFilter(string name, string field, string path, TValue value) : base(name, field, path)
        {
            Value = value;
        }

        public TValue Value { get; set; }

        public QueryContainer ToFilter(QueryContainerDescriptor<T> queryContainerDescriptor)
        {
            QueryContainer query = null;
            var isNumber = int.TryParse(Value?.ToString(), out var value);

            if ((!isNumber || value > 0) && !string.IsNullOrEmpty(Value?.ToString()))
            {
                return queryContainerDescriptor.Nested(n => n
                        .Path(Path)
                            .Query(q => q
                                .Term(Path + "." + Field, Value.ToString())));
            }

            return query;
        }

        public override AggregationContainerDescriptor<T> ToAggregationContainerDescriptor(AggregationContainerDescriptor<T> aggregationDescriptor) =>
            aggregationDescriptor
                .Nested(Name, n => n
                .Path(Path)
                    .Aggregations(a => a
                        .Terms(Name, t => t.Field(Path + "." + Field))));
    }

    public class NestedSearchMultiValueFilter<T, TValue, TValue2> : NestedSearchFilter<T> where T : class
    {
        public TValue2 FilterValue { get; set; }
        public string FilterField { get; set; }
        public NestedSearchMultiValueFilter(string name, string field1, string path, string filterField, TValue value, TValue2 filterValue) : base(name, field1, path)
        {
            Value = value;
            FilterValue = filterValue;
            FilterField = filterField;
        }

        public TValue Value { get; set; }

        public QueryContainer ToFilter(QueryContainerDescriptor<T> queryContainerDescriptor)
        {
            QueryContainer query = null;
            var isNumber = int.TryParse(Value?.ToString(), out var value);

            if ((!isNumber || value > 0) && !string.IsNullOrEmpty(Value?.ToString()))
            {
                var musts = new List<QueryContainer>();
                var queryDesc = new QueryContainerDescriptor<T>();

                musts.Add(queryDesc.Term(Path + "." + FilterField, FilterValue));
                musts.Add(queryDesc.Term(Path + "." + Field, Value.ToString()));

                return queryContainerDescriptor.Nested(n => n
                        .Path(Path)
                            .Query(q => q
                                .Bool(b => b.Must(musts.ToArray()))));
            }

            return query;
        }

        public override AggregationContainerDescriptor<T> ToAggregationContainerDescriptor(AggregationContainerDescriptor<T> aggregationDescriptor)
            => aggregationDescriptor
                .Filter(Name, f => f.Filter(f => f.Term(Path + "." + FilterField, FilterValue)))
                .Nested(Name, n => n
                .Path(Path)
                    .Aggregations(a => a
                        .Terms(Name, t => t.Field(Path + "." + Field))));
    }
}
