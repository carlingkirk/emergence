using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nest;

namespace Emergence.Service.Search
{
    public interface ISearchClient<T> where T : class
    {
        Task ConfigureClient(string indexName, string alias, Func<ClrTypeMappingDescriptor<T>, IClrTypeMapping<T>> selector, Func<TypeMappingDescriptor<T>, ITypeMapping> mappingSelector,
            Func<IndexSettingsDescriptor, IPromise<IndexSettings>> settingSelector);
        Task<bool> IndexAsync(T document);
        Task<BulkIndexResponse> IndexManyAsync(IEnumerable<T> documents);
        Task<SearchResponse<T>> SearchAsync(Func<SearchDescriptor<T>, ISearchRequest> search);
    }
}
