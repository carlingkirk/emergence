using System;
using System.Linq;
using System.Linq.Expressions;
using Emergence.Data;
using Emergence.Data.Shared.Stores;
using Moq;

namespace Emergence.Test.Mocks
{
    public static class RepositoryMocks
    {
        public static Mock<IRepository<Specimen>> GetStandardMockSpecimenRepository()
        {
            var mockSpecimenRepo = new Mock<IRepository<Specimen>>();

            mockSpecimenRepo.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Specimen, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(Data.Fakes.Stores.FakeSpecimens.Get().First());

            mockSpecimenRepo.Setup(p => p.GetSomeAsync(It.IsAny<Expression<Func<Specimen, bool>>>(), It.IsAny<bool>()))
                .Returns(Data.Fakes.Stores.FakeSpecimens.Get().ToAsyncEnumerable());

            return mockSpecimenRepo;
        }

        public static Mock<IRepository<Lifeform>> GetStandardMockLifeformRepository()
        {
            var mockLifeformItemRepo = new Mock<IRepository<Lifeform>>();

            mockLifeformItemRepo.Setup(p => p.GetSomeAsync(It.IsAny<Expression<Func<Lifeform, bool>>>(), It.IsAny<bool>()))
                .Returns(Data.Fakes.Stores.FakeLifeforms.Lifeforms().ToAsyncEnumerable());

            mockLifeformItemRepo.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Lifeform, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(Data.Fakes.Stores.FakeLifeforms.Lifeforms().First());

            return mockLifeformItemRepo;
        }

        public static Mock<IRepository<InventoryItem>> GetStandardMockInventoryItemRepository()
        {
            var mockInventoryItemRepo = new Mock<IRepository<InventoryItem>>();

            mockInventoryItemRepo.Setup(p => p.GetSomeAsync(It.IsAny<Expression<Func<InventoryItem, bool>>>(), It.IsAny<bool>()))
                .Returns(Data.Fakes.Stores.FakeInventories.GetItems().ToAsyncEnumerable());

            return mockInventoryItemRepo;
        }

        public static Mock<IRepository<Inventory>> GetStandardMockInventoryRepository()
        {
            var mockInventoryRepo = new Mock<IRepository<Inventory>>();

            mockInventoryRepo.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Inventory, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(Data.Fakes.Stores.FakeInventories.Get().First());

            return mockInventoryRepo;
        }

        public static Mock<IRepository<PlantInfo>> GetStandardMockPlantInfoRepository()
        {
            var mockPlantInfoRepo = new Mock<IRepository<PlantInfo>>();

            mockPlantInfoRepo.Setup(p => p.GetAsync(It.IsAny<Expression<Func<PlantInfo, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(Data.Fakes.Stores.FakePlantInfos.Get().First());

            mockPlantInfoRepo.Setup(p => p.GetSomeAsync(It.IsAny<Expression<Func<PlantInfo, bool>>>(), It.IsAny<bool>()))
                .Returns(Data.Fakes.Stores.FakePlantInfos.Get().ToAsyncEnumerable());

            mockPlantInfoRepo.Setup(p => p.AddOrUpdateAsync(It.IsAny<Expression<Func<PlantInfo, bool>>>(), It.IsAny<PlantInfo>()))
                .ReturnsAsync(Data.Fakes.Stores.FakePlantInfos.Get().First());

            return mockPlantInfoRepo;
        }
    }
}
