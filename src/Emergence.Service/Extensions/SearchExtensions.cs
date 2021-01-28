using System.Collections.Generic;
using System.Linq;
using Emergence.Service.Search;
using Nest;

namespace Emergence.Service.Extensions
{
    public static class SearchExtensions
    {
        public static AggregationContainerDescriptor<T> ToAggregationDescriptor<T>(this AggregationContainerDescriptor<T> aggs, List<QueryContainer> filters,
            SearchFilter<T> searchFilter) where T : class
        {
            var aggregation = searchFilter.ToAggregationContainerDescriptor();

            return aggs
                .Filter(searchFilter.Name, af => af
                    .Filter(f =>
                    {
                        QueryContainer qc = null;
                        if (!filters.Any())
                        {
                            qc &= f.MatchAll();
                        }

                        if (filters.Any())
                        {
                            qc &= f.Bool(b => b
                                .Must(filters.ToArray()));
                            return qc;
                        }
                        return qc;
                    })
                    .Aggregations(a => aggregation));
        }

        public static AggregationContainerDescriptor<T> ToAggregationDescriptor<T>(this AggregationContainerDescriptor<T> aggs, List<QueryContainer> filters,
            NestedSearchFilter<T> nestedSearchFilter) where T : class
        {
            var ad = nestedSearchFilter.ToAggregationContainerDescriptor();

            return aggs
                .Filter(nestedSearchFilter.Name, af => af
                    .Filter(f =>
                    {
                        QueryContainer query = null;
                        if (!filters.Any())
                        {
                            query &= f.MatchAll();
                        }

                        if (filters.Any())
                        {
                            query &= f.Bool(b => b
                                .Must(filters.ToArray()));
                            return query;
                        }
                        return query;
                    })
                    .Aggregations(a => ad));
        }
    }
}
