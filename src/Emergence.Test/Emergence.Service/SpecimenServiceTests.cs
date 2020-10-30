using System.Linq;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;
using Emergence.Service;
using Emergence.Test.Data.Fakes.Stores;
using Emergence.Test.Mocks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Emergence.Test.API.Services
{
    public class SpecimenServiceTests : TestBase
    {
        private readonly Mock<IRepository<Specimen>> _mockSpecimenRepository;
        private readonly ILogger _logger;
        public SpecimenServiceTests()
        {
            _mockSpecimenRepository = RepositoryMocks.GetStandardMockSpecimenRepository();
            _logger = GetLogger<SpecimenService>();
        }

        [Fact]
        public async Task TestGetSpecimenAsync()
        {
            var specimenService = new SpecimenService(_mockSpecimenRepository.Object);
            var specimen = await specimenService.GetSpecimenAsync(1, FakeUsers.GetPublic().AsModel());

            specimen.Should().NotBeNull("it exists");
        }

        [Fact]
        public async Task TestFindSpecimens()
        {
            var specimenService = new SpecimenService(_mockSpecimenRepository.Object);
            var specimenResult = await specimenService.FindSpecimens(new FindParams
            {
                SearchText = "Liatris spicata",
                Skip = 0,
                Take = 10,
                SortBy = "",
                SortDirection = SortDirection.None,
            },
            FakeUsers.GetPublic().AsModel());

            specimenResult.Results.Should().NotBeNull("it exists");
            specimenResult.Count.Should().Be(2);
            specimenResult.Results.Should().HaveCount(2);
            specimenResult.Results.First().InventoryItem.Should().NotBeNull();
            specimenResult.Results.First().InventoryItem.Inventory.Should().NotBeNull();
            specimenResult.Results.First().Lifeform.Should().NotBeNull();
        }
    }
}
