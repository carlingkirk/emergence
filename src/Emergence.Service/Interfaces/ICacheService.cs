using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Emergence.Service.Interfaces
{
    public interface ICacheService
    {
        Task<int?> GetIntAsync(string key);
        Task<string> GetStringAsync(string key);
        Task SetCacheValueAsync<T>(string key, T value, DistributedCacheEntryOptions options = null);
    }
}
