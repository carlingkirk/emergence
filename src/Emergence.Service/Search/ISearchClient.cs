using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nest;

namespace Emergence.Service.Search
{
    public interface ISearchClient<T> where T : class
    {
        Task ConfigureClient(string indexName, Func<ClrTypeMappingDescriptor<T>, IClrTypeMapping<T>> selector);
        Task<bool> IndexAsync(T document);
        Task<BulkIndexResponse> IndexManyAsync(IEnumerable<T> documents);
        Task CreateIndexAsync(string indexName);
        Task<SearchResponse<T>> SearchAsync(Func<QueryContainerDescriptor<T>, QueryContainer> query, int skip, int take);
    }
}
