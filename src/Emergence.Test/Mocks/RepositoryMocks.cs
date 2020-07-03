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

            mockSpecimenRepo.Setup(i => i.GetAsync(It.IsAny<Expression<Func<Specimen, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(Data.Fakes.Stores.FakeSpecimens.Get().First());

            mockSpecimenRepo.Setup(i => i.GetSomeAsync(It.IsAny<Expression<Func<Specimen, bool>>>(), It.IsAny<bool>()))
                .Returns(Data.Fakes.Stores.FakeSpecimens.Get().ToAsyncEnumerable());

            return mockSpecimenRepo;
        }

        public static Mock<IRepository<Lifeform>> GetStandardMockLifeformRepository()
        {
            var mockLifeformItemRepo = new Mock<IRepository<Lifeform>>();

            mockLifeformItemRepo.Setup(i => i.GetSomeAsync(It.IsAny<Expression<Func<Lifeform, bool>>>(), It.IsAny<bool>()))
                .Returns(Data.Fakes.Stores.FakeLifeforms.Lifeforms().ToAsyncEnumerable());

            return mockLifeformItemRepo;
        }

        public static Mock<IRepository<InventoryItem>> GetStandardMockInventoryItemRepository()
        {
            var mockInventoryItemRepo = new Mock<IRepository<InventoryItem>>();

            mockInventoryItemRepo.Setup(i => i.GetSomeAsync(It.IsAny<Expression<Func<InventoryItem, bool>>>(), It.IsAny<bool>()))
                .Returns(Data.Fakes.Stores.FakeInventories.GetItems().ToAsyncEnumerable());

            return mockInventoryItemRepo;
        }

        public static Mock<IRepository<Inventory>> GetStandardMockInventoryRepository()
        {
            var mockInventoryRepo = new Mock<IRepository<Inventory>>();

            mockInventoryRepo.Setup(i => i.GetAsync(It.IsAny<Expression<Func<Inventory, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(Data.Fakes.Stores.FakeInventories.Get().First());

            return mockInventoryRepo;
        }
    }
}
