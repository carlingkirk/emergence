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

        [Fact]
        public async Task TestGetSomeAsync()
        {
            var mockContext = new DbContextMock<EmergenceDbContext>();
            var specimensDbSetMock = mockContext.CreateDbSetMock(x => x.Specimens, FakeSpecimens.Get());
            mockContext.Setup(c => c.Set<Specimen>()).Returns(specimensDbSetMock.Object);

            var specimenRepository = new Repository<Specimen>(mockContext.Object);
            var specimen = await specimenRepository.GetAsync(s => s.Id == 1);

            specimen.InventoryItemId.Should().Be(1);
            specimen.SpecimenStage.Should().Be("Seed");
            specimen.InventoryItem.Should().NotBeNull();
        }
    }
}
