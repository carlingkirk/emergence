using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Caching.Distributed;
using Moq;

namespace Emergence.Test.Mocks
{
    public static class CacheMocks
    {
        public static IDistributedCache GetDistributedCache(Dictionary<string, byte[]> keyValues = null)
        {
            var mockCache = new Mock<IDistributedCache>();
            mockCache.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((string key, CancellationToken token) =>
            {
                if (keyValues.TryGetValue(key, out var value))
                {
                    return value;
                }
                return null;
            });

            return mockCache.Object;
        }
    }
}
