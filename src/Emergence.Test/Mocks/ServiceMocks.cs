using System.Collections.Generic;
using System.Linq;
using Emergence.Data.Shared.Models;
using Emergence.Service.Interfaces;
using Emergence.Test.Data.Fakes.Models;
using Moq;

namespace Emergence.Test.Mocks
{
    public static class ServiceMocks
    {
        public static Mock<ISpecimenService> GetStandardMockSpecimenService(IEnumerable<Specimen> result = null)
        {
            var mockSpecimenService = new Mock<ISpecimenService>();

            mockSpecimenService.Setup(s => s.GetSpecimensByIdsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(result ?? FakeSpecimens.Get());

            return mockSpecimenService;
        }

        public static Mock<IInventoryService> GetStandardMockInventoryService(IEnumerable<InventoryItem> itemsResult = null)
        {
            var mockInventoryService = new Mock<IInventoryService>();

            mockInventoryService.Setup(s => s.GetInventoryItemsByIdsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(itemsResult ?? FakeInventories.Get().First().Items);

            return mockInventoryService;
        }

        public static Mock<IOriginService> GetStandardMockOriginService(IEnumerable<Origin> originsResult = null)
        {
            var mockOriginService = new Mock<IOriginService>();

            mockOriginService.Setup(s => s.GetOriginAsync(It.IsAny<int>()))
                .ReturnsAsync(originsResult?.FirstOrDefault() ?? FakeOrigins.Get().First());

            mockOriginService.Setup(s => s.GetOriginsAsync())
                .ReturnsAsync(originsResult ?? FakeOrigins.Get());

            return mockOriginService;
        }
    }
}
