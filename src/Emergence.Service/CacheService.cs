using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Emergence.Service.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Emergence.Service
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<UserService> _logger;

        public CacheService(IDistributedCache cache, ILogger<UserService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<int?> GetIntAsync(string key)
        {
            var bytes = await _cache.GetAsync(key);

            if (bytes == null)
            {
                _logger.LogDebug($"Cache miss: {key}");
                return null;
            }
            else
            {
                _logger.LogDebug($"Cache hit: {key}");
                using (var memoryStream = new MemoryStream(bytes))
                {
                    var binaryReader = new BinaryReader(memoryStream);
                    return binaryReader.ReadInt32();
                }
            }
        }

        public async Task<string> GetStringAsync(string key)
        {
            var bytes = await _cache.GetAsync(key);

            if (bytes == null)
            {
                _logger.LogDebug($"Cache miss: {key}");
                return null;
            }
            else
            {
                _logger.LogDebug($"Cache hit: {key}");
                using (var memoryStream = new MemoryStream(bytes))
                {
                    var binaryReader = new BinaryReader(memoryStream);
                    return binaryReader.ReadString();
                }
            }
        }

        public async Task SetCacheValueAsync<T>(string key, T value, DistributedCacheEntryOptions options = null)
        {
            if (options == null)
            {
                options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };
            }

            byte[] result;
            switch (value)
            {
                case int intValue:
                    result = BitConverter.GetBytes(intValue);
                    break;
                case string stringValue:
                    result = Encoding.UTF8.GetBytes(stringValue);
                    break;
                default:
                    throw new NotImplementedException();
            }

            await _cache.SetAsync(key, result, options);
        }
    }
}
