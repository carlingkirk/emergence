using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Emergence.Service.Extensions
{
    public static class CacheExtensions
    {
        public static async Task<int?> GetIntAsync(this IDistributedCache cache, string key)
        {
            var bytes = await cache.GetAsync(key);

            if (bytes == null)
            {
                return null;
            }

            using (var memoryStream = new MemoryStream(bytes))
            {
                var binaryReader = new BinaryReader(memoryStream);
                return binaryReader.ReadInt32();
            }
        }

        public static async Task<string> GetStringAsync(this IDistributedCache cache, string key)
        {
            var bytes = await cache.GetAsync(key);

            if (bytes == null)
            {
                return null;
            }

            using (var memoryStream = new MemoryStream(bytes))
            {
                var binaryReader = new BinaryReader(memoryStream);
                return binaryReader.ReadString();
            }
        }

        public static async Task SetCacheValueAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options = null)
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

            await cache.SetAsync(key, result, options);
        }
    }
}
