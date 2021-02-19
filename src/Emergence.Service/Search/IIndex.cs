using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared;

namespace Emergence.Service.Search
{
    public interface IIndex<T, TResult>
    {
        Task<bool> IndexAsync(T document);
        Task<BulkIndexResponse> IndexManyAsync(IEnumerable<T> documents);
        Task<SearchResponse<T>> SearchAsync(FindParams<TResult> findParams, Data.Shared.Models.User user);
        Task<bool> RemoveAsync(string id);
    }
}
