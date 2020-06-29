using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Emergence.API.Services;
using Emergence.Data;
using Emergence.Data.Shared.Stores;
using FluentAssertions;
using Moq;
using Xunit;
using Models = Emergence.Data.Shared.Models;

namespace Emergence.Test.API.Services
{
    public class SpecimenServiceTests
    {
        private readonly Mock<IRepository<Specimen>> _mockSpecimenRepository;
        public SpecimenServiceTests()
        {
            _mockSpecimenRepository = GetStandardMockSpecimenRepository();
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
            var specimens = await specimenService.GetSpecimensAsync(0);

            specimens.Should().NotBeNull("it exists");
            specimens.Should().HaveCount(3);
            specimens.Where(s => s.SpecimenStage == Models.SpecimenStage.Seed).Should().HaveCount(1);
            specimens.Where(s => s.SpecimenStage == Models.SpecimenStage.Stratification).Should().HaveCount(1);
            specimens.Where(s => s.SpecimenStage == Models.SpecimenStage.Growing).Should().HaveCount(1);
        }

        private Mock<IRepository<Specimen>> GetStandardMockSpecimenRepository()
        {
            var mockSpecimenRepo = new Mock<IRepository<Specimen>>();

            mockSpecimenRepo.Setup(i => i.GetAsync(It.IsAny<long>(), It.IsAny<bool>()))
                .ReturnsAsync(Data.Fakes.Stores.FakeSpecimens.Get().First());

            mockSpecimenRepo.Setup(i => i.GetSomeAsync(It.IsAny<Expression<Func<Specimen, bool>>>(), It.IsAny<bool>()))
                .Returns(Data.Fakes.Stores.FakeSpecimens.Get().ToAsyncEnumerable());

            return mockSpecimenRepo;
        }
    }
}
