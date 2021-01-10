using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared.Search.Models;
using Nest;

namespace Emergence.Service.Search
{
    public interface ISearchClient<T> where T : class
    {
        Task ConfigureClient(string indexName, Func<ClrTypeMappingDescriptor<T>, IClrTypeMapping<T>> selector);
        Task<IEnumerable<PlantInfo>> SearchAsync(string search);
        Task<bool> IndexAsync(T document);
        Task<BulkIndexResponse> IndexManyAsync(IEnumerable<T> documents);
        Task CreateIndexAsync(string indexName);
    }
}
