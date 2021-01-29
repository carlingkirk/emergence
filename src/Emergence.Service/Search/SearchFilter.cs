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

        public override AggregationContainerDescriptor<T> ToAggregationContainerDescriptor(AggregationContainerDescriptor<T> aggregationDescriptor) =>
            aggregationDescriptor
                .Terms(Name, t => t
                    .Field(Field));
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

        public override AggregationContainerDescriptor<T> ToAggregationContainerDescriptor(AggregationContainerDescriptor<T> aggregationDescriptor)
        {
            if (string.IsNullOrEmpty(Value?.ToString()))
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
