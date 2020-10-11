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
        public static Mock<ILifeformService> GetStandardMockLifeformService(IEnumerable<Lifeform> result = null)
        {
            var mockLifeformService = new Mock<ILifeformService>();

            mockLifeformService.Setup(s => s.GetLifeformByScientificNameAsync(It.IsAny<string>()))
                .ReturnsAsync(result.FirstOrDefault() ?? FakeLifeforms.Get().First());

            return mockLifeformService;
        }

        public static Mock<IPhotoService> GetStandardMockPhotoService(IEnumerable<Photo> result = null)
        {
            var mockPhotoService = new Mock<IPhotoService>();
            if (result == null)
            {
                result = FakePhotos.Get();
            }

            mockPhotoService.Setup(s => s.GetPhotoAsync(It.IsAny<int>()))
                .ReturnsAsync(result.FirstOrDefault());

            return mockPhotoService;
        }

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

        public static Mock<ILocationService> GetStandardMockLocationService(IEnumerable<Location> result = null)
        {
            var mockLocationService = new Mock<ILocationService>();

            mockLocationService.Setup(l => l.GetLocationsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(result ?? FakeLocations.Get());

            return mockLocationService;
        }

        public static Mock<ITaxonService> GetStandardMockTaxonService(IEnumerable<Taxon> result = null)
        {
            var mockTaxonService = new Mock<ITaxonService>();

            mockTaxonService.Setup(l => l.GetTaxonAsync(It.IsAny<int>()))
                .ReturnsAsync(result.FirstOrDefault() ?? FakeTaxons.Get().First());

            return mockTaxonService;
        }
    }
}
