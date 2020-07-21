using System;
using System.Linq;
using System.Linq.Expressions;
using Emergence.Data;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Stores;
using MockQueryable.Moq;
using Moq;

namespace Emergence.Test.Mocks
{
    public static class RepositoryMocks
    {
        public static Mock<IRepository<Specimen>> GetStandardMockSpecimenRepository()
        {
            var mockSpecimenRepo = new Mock<IRepository<Specimen>>();
            var mockSpecimens = Data.Fakes.Stores.FakeSpecimens.Get().AsQueryable().BuildMockDbSet<Specimen>().Object;

            mockSpecimenRepo.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Specimen, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(mockSpecimens.First());

            mockSpecimenRepo.Setup(p => p.GetWithIncludesAsync(It.IsAny<Expression<Func<Specimen, bool>>>(), It.IsAny<bool>(),
                It.IsAny<Func<IIncludable<Specimen>, IIncludable>[]>()))
                .ReturnsAsync(mockSpecimens.First());

            mockSpecimenRepo.Setup(p => p.Where(It.IsAny<Expression<Func<Specimen, bool>>>()))
                .Returns(mockSpecimens);

            mockSpecimenRepo.Setup(p => p.WhereWithIncludesAsync(It.IsAny<Expression<Func<Specimen, bool>>>(),
                It.IsAny<Func<IIncludable<Specimen>, IIncludable>[]>()))
                .Returns(mockSpecimens);

            mockSpecimenRepo.Setup(p => p.GetSomeAsync(It.IsAny<Expression<Func<Specimen, bool>>>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<bool>()))
                .Returns(mockSpecimens);

            return mockSpecimenRepo;
        }

        public static Mock<IRepository<Origin>> GetStandardMockOriginRepository()
        {
            var mockOriginRepo = new Mock<IRepository<Origin>>();

            mockOriginRepo.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Origin, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(Data.Fakes.Stores.FakeOrigins.Get().First());

            mockOriginRepo.Setup(p => p.GetSomeAsync(It.IsAny<Expression<Func<Origin, bool>>>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<bool>()))
                .Returns(Data.Fakes.Stores.FakeOrigins.Get().ToAsyncEnumerable());

            return mockOriginRepo;
        }

        public static Mock<IRepository<Lifeform>> GetStandardMockLifeformRepository()
        {
            var mockLifeformItemRepo = new Mock<IRepository<Lifeform>>();

            mockLifeformItemRepo.Setup(p => p.GetSomeAsync(It.IsAny<Expression<Func<Lifeform, bool>>>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<bool>()))
                .Returns(Data.Fakes.Stores.FakeLifeforms.Get().ToAsyncEnumerable());

            mockLifeformItemRepo.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Lifeform, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(Data.Fakes.Stores.FakeLifeforms.Get().First());

            return mockLifeformItemRepo;
        }

        public static Mock<IRepository<InventoryItem>> GetStandardMockInventoryItemRepository()
        {
            var mockInventoryItemRepo = new Mock<IRepository<InventoryItem>>();

            mockInventoryItemRepo.Setup(p => p.GetSomeAsync(It.IsAny<Expression<Func<InventoryItem, bool>>>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<bool>()))
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

            mockPlantInfoRepo.Setup(p => p.GetSomeAsync(It.IsAny<Expression<Func<PlantInfo, bool>>>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<bool>()))
                .Returns(Data.Fakes.Stores.FakePlantInfos.Get().ToAsyncEnumerable());

            mockPlantInfoRepo.Setup(p => p.AddOrUpdateAsync(It.IsAny<Expression<Func<PlantInfo, bool>>>(), It.IsAny<PlantInfo>()))
                .ReturnsAsync(Data.Fakes.Stores.FakePlantInfos.Get().First());

            return mockPlantInfoRepo;
        }

        public static Mock<IRepository<Activity>> GetStandardMockActivityRepository()
        {
            var mockActivityRepo = new Mock<IRepository<Activity>>();

            mockActivityRepo.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<bool>()))
                .ReturnsAsync(Data.Fakes.Stores.FakeActivities.Get().First());

            mockActivityRepo.Setup(p => p.GetSomeAsync(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<bool>()))
                .Returns(Data.Fakes.Stores.FakeActivities.Get().ToAsyncEnumerable());

            return mockActivityRepo;
        }
    }
}
