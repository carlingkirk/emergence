using System.Collections.Generic;
using System.Linq;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Models;
using Emergence.Service.Interfaces;
using Emergence.Test.Data.Fakes.Stores;
using Microsoft.Extensions.Caching.Distributed;
using Moq;

namespace Emergence.Test.Mocks
{
    public static class ServiceMocks
    {
        public static Mock<ILifeformService> GetStandardMockLifeformService(IEnumerable<Lifeform> result = null)
        {
            var mockLifeformService = new Mock<ILifeformService>();

            mockLifeformService.Setup(s => s.GetLifeformByScientificNameAsync(It.IsAny<string>()))
                .ReturnsAsync(result.FirstOrDefault() ?? FakeLifeforms.Get().First().AsModel());

            return mockLifeformService;
        }

        public static Mock<IPhotoService> GetStandardMockPhotoService(IEnumerable<Photo> result = null)
        {
            var mockPhotoService = new Mock<IPhotoService>();
            if (result == null)
            {
                result = FakePhotos.Get().Select(p => p.AsModel("https://blobs.com/photos/"));
            }

            mockPhotoService.Setup(s => s.GetPhotoAsync(It.IsAny<int>()))
                .ReturnsAsync(result.FirstOrDefault());

            return mockPhotoService;
        }

        public static Mock<ISpecimenService> GetStandardMockSpecimenService(IEnumerable<Specimen> result = null)
        {
            var mockSpecimenService = new Mock<ISpecimenService>();

            mockSpecimenService.Setup(s => s.GetSpecimenAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(result.FirstOrDefault() ?? FakeSpecimens.Get().First().AsModel());

            return mockSpecimenService;
        }

        public static Mock<IInventoryService> GetStandardMockInventoryService(IEnumerable<InventoryItem> itemsResult = null)
        {
            var mockInventoryService = new Mock<IInventoryService>();

            mockInventoryService.Setup(s => s.GetInventoryItemsByIdsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(itemsResult ?? FakeInventories.GetItems().Select(i => i.AsModel()));

            return mockInventoryService;
        }

        public static Mock<IOriginService> GetStandardMockOriginService(IEnumerable<Origin> originsResult = null)
        {
            var mockOriginService = new Mock<IOriginService>();

            mockOriginService.Setup(s => s.GetOriginAsync(It.IsAny<int>()))
                .ReturnsAsync(originsResult?.FirstOrDefault() ?? FakeOrigins.Get().First().AsModel());

            return mockOriginService;
        }

        public static Mock<ILocationService> GetStandardMockLocationService(IEnumerable<Location> result = null)
        {
            var mockLocationService = new Mock<ILocationService>();

            mockLocationService.Setup(l => l.GetLocationsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(result ?? FakeLocations.Get().Select(l => l.AsModel()));

            return mockLocationService;
        }

        public static Mock<ITaxonService> GetStandardMockTaxonService(IEnumerable<Taxon> result = null)
        {
            var mockTaxonService = new Mock<ITaxonService>();

            mockTaxonService.Setup(l => l.GetTaxonAsync(It.IsAny<int>()))
                .ReturnsAsync(result.FirstOrDefault() ?? FakeTaxons.Get().First().AsModel());

            return mockTaxonService;
        }

        public static Mock<IUserService> GetStandardMockUserService(IEnumerable<User> result = null)
        {
            if (result == null)
            {
                result = FakeUsers.Get().Select(u => u.AsModel());
            }

            var mockUserService = new Mock<IUserService>();

            mockUserService.Setup(u => u.GetIdentifyingUser(It.IsAny<string>()))
                .ReturnsAsync((string userId) => result.FirstOrDefault(u => u.UserId == userId));

            mockUserService.Setup(u => u.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync((string userId) => result.FirstOrDefault(u => u.UserId == userId));

            mockUserService.Setup(u => u.GetUserByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((string name) => result.FirstOrDefault(u => u.DisplayName == name));

            mockUserService.Setup(u => u.GetUserIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string userId) => result.FirstOrDefault(u => u.UserId == userId).Id);

            mockUserService.Setup(u => u.GetRandomNameAsync())
                .ReturnsAsync("ElatedScurfpea");

            mockUserService.Setup(u => u.AddUserAsync(It.IsAny<User>()))
                .ReturnsAsync((User user) => user);

            mockUserService.Setup(u => u.UpdateUserAsync(It.IsAny<User>()))
                .ReturnsAsync((User user) => user);

            return mockUserService;
        }

        public static Mock<ICacheService> GetStandardMockCacheService(Dictionary<string, string> keyValues = null)
        {
            var mockCacheService = new Mock<ICacheService>();
            mockCacheService.Setup(c => c.GetIntAsync(It.IsAny<string>())).ReturnsAsync((string key) =>
            {
                if (keyValues.TryGetValue(key, out var value))
                {
                    if (int.TryParse(value, out var intValue))
                    {
                        return intValue;
                    }
                }
                return null;
            });

            mockCacheService.Setup(c => c.GetStringAsync(It.IsAny<string>())).ReturnsAsync((string key) =>
            {
                if (keyValues.TryGetValue(key, out var value))
                {
                    return value;
                }
                return null;
            });

            mockCacheService.Setup(c => c.SetCacheValueAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DistributedCacheEntryOptions>()));
            mockCacheService.Setup(c => c.SetCacheValueAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DistributedCacheEntryOptions>()));

            return mockCacheService;
        }
    }
}
