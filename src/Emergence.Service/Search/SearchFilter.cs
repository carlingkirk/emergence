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

        public QueryContainer GetSearchFilter(object value) =>
            new QueryContainerDescriptor<T>().Terms(t => t
                .Field(Field)
                .Terms(value));

        public virtual AggregationContainerDescriptor<T> ToAggregationContainerDescriptor() =>
            new AggregationContainerDescriptor<T>()
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

        public override AggregationContainerDescriptor<T> ToAggregationContainerDescriptor() =>
            new AggregationContainerDescriptor<T>()
                .Nested(Name, n => n
                    .Path(Path)
                    .Aggregations(ca => ca
                        .Terms(Name, t => t
                            .Field(Path + "." + Field))));
    }
}
