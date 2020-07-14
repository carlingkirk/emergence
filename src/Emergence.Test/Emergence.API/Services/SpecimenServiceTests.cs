using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Emergence.API.Services;
using Emergence.API.Services.Interfaces;
using Emergence.Data;
using Emergence.Data.Shared.Stores;
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
        private readonly Mock<IInventoryService> _mockInventoryService;
        private readonly ILogger _logger;
        public SpecimenServiceTests()
        {
            _mockSpecimenRepository = RepositoryMocks.GetStandardMockSpecimenRepository();
            _mockInventoryService = ServiceMocks.GetStandardMockInventoryService();
            _logger = GetLogger<SpecimenService>();
        }

        [Fact]
        public async Task TestGetSpecimenAsync()
        {
            var specimenService = new SpecimenService(_mockSpecimenRepository.Object, _mockInventoryService.Object);
            var specimen = await specimenService.GetSpecimenAsync(0);

            specimen.Should().NotBeNull("it exists");
        }

        [Fact]
        public async Task TestGetSpecimensAsync()
        {
            var specimenService = new SpecimenService(_mockSpecimenRepository.Object, _mockInventoryService.Object);
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
            _mockSpecimenRepository.Setup(p => p.GetSomeAsync(It.IsAny<Expression<Func<Specimen, bool>>>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<bool>()))
                .Returns(Data.Fakes.Stores.FakeSpecimens.Get().Take(1).ToAsyncEnumerable());

            var specimenService = new SpecimenService(_mockSpecimenRepository.Object, _mockInventoryService.Object);
            var specimens = await specimenService.FindSpecimens("Liatris spicata", "me");

            specimens.Should().NotBeNull("it exists");
            specimens.Should().HaveCount(3);
            specimens.First().InventoryItem.Should().NotBeNull();
            specimens.First().InventoryItem.Inventory.Should().NotBeNull();
            specimens.First().Lifeform.Should().NotBeNull();
        }
    }
}
