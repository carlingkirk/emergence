using System.Collections.Generic;
using System.Linq;
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

    public class SearchRangeFilter<T> : SearchFilter<T> where T : class
    {
        public string MinField { get; set; }
        public string MaxField { get; set; }
        public IEnumerable<int> Values { get; set; }

        public SearchRangeFilter(string name, string minField, string maxField, IEnumerable<int> values, string field = null) : base(name, field)
        {
            Name = name;
            MinField = minField;
            MaxField = maxField;
            Values = values;
        }

        public override AggregationContainerDescriptor<T> ToAggregationContainerDescriptor(AggregationContainerDescriptor<T> aggregationDescriptor)
        {
            var values = Values.ToArray();

            for (var i = 0; i < values.Length; i++)
            {
                if (i + 1 <= values.Length)
                {
                    aggregationDescriptor.Filters(Name, ff => ff
                        .NamedFilters(nf => nf
                            .Filter(i.ToString(),
                                    f => (f.Exists(e => e.Field(MinField)) || f.Exists(e => e.Field(MaxField))) &&
                                         (f.Range(r => r.Field(MinField).LessThanOrEquals(i)) ||
                                          (!f.Exists(e => e.Field(MinField)) &&
                                          (f.Range(r => r.Field(MaxField).GreaterThanOrEquals(i)) ||
                                           !f.Exists(e => e.Field(MaxField))))))));
                }
            }

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

        public override AggregationContainerDescriptor<T> ToAggregationContainerDescriptor(AggregationContainerDescriptor<T> aggregationDescriptor)
        {
            if (string.IsNullOrEmpty(Value?.ToString()))
            {
                return aggregationDescriptor
                 .Terms(Name, t => t
                     .Field(Field));
            }
            else
            {
                return aggregationDescriptor
                    .Filter(Name, f => f.Filter(ff => ff.Term(Name, Value)))
                 .Terms(Name, t => t
                     .Field(Field));
            }
        }
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
                                .Match(sq => sq
                                    .Field(Field)
                                    .Query(Value.ToString())
                                    .Fuzziness(Fuzziness.AutoLength(1, 5)))));
            }

            return query;
        }

        public override AggregationContainerDescriptor<T> ToAggregationContainerDescriptor(AggregationContainerDescriptor<T> aggregationDescriptor)
        {
            var isNumber = int.TryParse(Value?.ToString(), out var value);

            if ((isNumber && value == 0) || string.IsNullOrEmpty(Value?.ToString()))
            {
                return aggregationDescriptor
                    .Nested(Name, n => n
                    .Path(Path)
                        .Aggregations(a => a
                            .Terms(Name, t => t.Field(Path + "." + Field))));
            }
            else
            {
                return aggregationDescriptor
                    .Filter(Name, f => f.Filter(f => f
                        .Nested(n => n.Path(Path).Query(q => q.Term(Path + "." + Field, Value.ToString()))))
                    .Aggregations(a => a
                        .Terms(Name, t => t.Field(Path + "." + Field))));
            }
        }
    }
}
