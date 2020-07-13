using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Repository;
using Emergence.Data.Shared.Stores;
using Emergence.Test.Data.Fakes.Stores;
using EntityFrameworkCore3Mock;
using FluentAssertions;
using Xunit;

namespace Emergence.Test.Data.Database
{
    public class RepositoryTests
    {
        private readonly DbContextMock<EmergenceDbContext> MockEmergenceDbContext;

        public RepositoryTests()
        {
            MockEmergenceDbContext = GetMockEmergenceDbContext(FakeSpecimens.Get());
        }

        [Fact(Skip = "Need to work on mocking Include")]
        public async Task TestGetSomeWithIncludeAsync()
        {
            var inventoryItemsDbSetMock = MockEmergenceDbContext.CreateDbSetMock(x => x.InventoryItems, FakeInventories.GetItems());
            MockEmergenceDbContext.Setup(c => c.Set<InventoryItem>()).Returns(inventoryItemsDbSetMock.Object);

            var inventoriesDbSetMock = MockEmergenceDbContext.CreateDbSetMock(x => x.Inventories, FakeInventories.Get());
            MockEmergenceDbContext.Setup(c => c.Set<Inventory>()).Returns(inventoriesDbSetMock.Object);

            var lifeformsDbSetMock = MockEmergenceDbContext.CreateDbSetMock(x => x.Lifeforms, FakeLifeforms.Get());
            MockEmergenceDbContext.Setup(c => c.Set<Lifeform>()).Returns(lifeformsDbSetMock.Object);

            var specimenRepository = new Repository<Specimen>(MockEmergenceDbContext.Object);
            var specimen = await specimenRepository.GetAsync(s => s.Id == 1, track: false);

            specimen.InventoryItemId.Should().Be(1);
            specimen.SpecimenStage.Should().Be("Seed");
            specimen.InventoryItem.Should().NotBeNull();
            specimen.InventoryItem.Inventory.Should().NotBeNull();
            specimen.Lifeform.Should().NotBeNull();
        }

        [Fact(Skip = "Need to work on mocking Include")]
        public async Task TestGetSomeLazyLoadingAsync()
        {
            var specimenRepository = new Repository<Specimen>(MockEmergenceDbContext.Object);
            var specimen = await specimenRepository.GetAsync(s => s.Id == 1, track: false);

            specimen.InventoryItemId.Should().Be(1);
            specimen.SpecimenStage.Should().Be("Seed");
            specimen.InventoryItem?.Should().BeNull("no include for InventoryItem was provided");
            specimen.InventoryItem?.Inventory?.Should().BeNull("no include for Inventory was provided");
            specimen.Lifeform?.Should().BeNull("no include for Lifeform was provided");
        }

        private DbContextMock<EmergenceDbContext> GetMockEmergenceDbContext(IEnumerable<Specimen> specimens)
        {
            var mockContext = new DbContextMock<EmergenceDbContext>();
            var specimensDbSetMock = new DbSetMock<Specimen>(specimens, (s, _) => s.Id, asyncQuerySupport: true);
            mockContext.Setup(c => c.Set<Specimen>()).Returns(specimensDbSetMock.Object);
            return mockContext;
        }
    }
}
