using System.Linq;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Stores;
using Emergence.Service;
using Emergence.Test.Mocks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Models = Emergence.Data.Shared.Models;

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
            var specimen = await specimenService.GetSpecimenAsync(0);

            specimen.Should().NotBeNull("it exists");
        }

        [Fact]
        public async Task TestGetSpecimensAsync()
        {
            var specimenService = new SpecimenService(_mockSpecimenRepository.Object);
            var specimens = await specimenService.GetSpecimensForInventoryAsync(0);

            specimens.Should().NotBeNull("it exists");
            specimens.Should().HaveCount(3);
            specimens.Where(s => s.SpecimenStage == Models.SpecimenStage.Seed).Should().HaveCount(1);
            specimens.Where(s => s.SpecimenStage == Models.SpecimenStage.Stratification).Should().HaveCount(1);
            specimens.Where(s => s.SpecimenStage == Models.SpecimenStage.Growing).Should().HaveCount(1);
        }

        [Fact]
        public async Task TestFindSpecimens()
        {
            var specimenService = new SpecimenService(_mockSpecimenRepository.Object);
            var specimenResult = await specimenService.FindSpecimens("Liatris spicata", "me", 0, 10, "", SortDirection.None);

            specimenResult.Results.Should().NotBeNull("it exists");
            specimenResult.Count.Should().Be(3);
            specimenResult.Results.Should().HaveCount(3);
            specimenResult.Results.First().InventoryItem.Should().NotBeNull();
            specimenResult.Results.First().InventoryItem.Inventory.Should().NotBeNull();
            specimenResult.Results.First().Lifeform.Should().NotBeNull();
        }
    }
}
