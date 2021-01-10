using System.Collections.Generic;
using System.Threading.Tasks;

namespace Emergence.Service.Search
{
    public interface IIndex<T>
    {
        Task CreateIndexAsync();
        Task<bool> IndexAsync(T document);
        Task<BulkIndexResponse> IndexManyAsync(IEnumerable<T> documents);
    }
}
