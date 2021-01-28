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
                // filter on region
                return aggregationDescriptor
                    .Filter(Name, f => f.Filter(f => f
                        .Nested(n => n.Path(Path).Query(q => q.Term(Path + "." + Field, Value.ToString()))))
                    .Aggregations(a => a
                        .Terms(Name, t => t.Field(Path + "." + Field))));
            }
        }
    }

    public class NestedSearchRangeFilter<T, TValue> : NestedSearchFilter<T> where T : class
    {
        public NestedSearchRangeFilter(string name, string field, string path, TValue minValue, TValue maxValue) : base(name, field, path)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public TValue MinValue { get; set; }
        public TValue MaxValue { get; set; }
    }
}
