using System.Linq;
using System.Threading.Tasks;
using Emergence.API.Services;
using Emergence.Data;
using Emergence.Data.Shared.Stores;
using FluentAssertions;
using Moq;
using Xunit;

namespace Emergence.Test.API.Services
{
    public class SpecimenServiceTests
    {
        [Fact]
        public async Task TestGet()
        {
            var mockSpecimenRepo = new Mock<IRepository<Specimen>>();

            mockSpecimenRepo.Setup(i => i.GetAsync(It.IsAny<long>(), It.IsAny<bool>()))
                .ReturnsAsync(Data.Fakes.Stores.FakeSpecimens.Get().First());

            var specimenService = new SpecimenService(mockSpecimenRepo.Object);
            var specimen = await specimenService.GetSpecimenAsync(0);

            specimen.Should().NotBeNull("it exists");
        }
    }
}
