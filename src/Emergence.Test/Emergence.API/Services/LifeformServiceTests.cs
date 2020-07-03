using System.Linq;
using System.Threading.Tasks;
using Emergence.API.Services;
using Emergence.Data;
using Emergence.Data.Shared.Stores;
using Emergence.Test.Mocks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Emergence.Test.API.Services
{
    public class LifeformServiceTests
    {
        private readonly Mock<IRepository<Lifeform>> _mockLifeformRepository;

        public LifeformServiceTests()
        {
            _mockLifeformRepository = RepositoryMocks.GetStandardMockLifeformRepository();
        }

        [Fact]
        public async Task TestGetLifeformAsync()
        {
            var lifeformsService = new LifeformService(_mockLifeformRepository.Object);
            var lifeforms = await lifeformsService.GetLifeformAsync(1);

            lifeforms.Should().NotBeNull("it exists");
        }

        [Fact]
        public async Task TestGetLifeformsAsync()
        {
            var lifeformService = new LifeformService(_mockLifeformRepository.Object);
            var lifeforms = await lifeformService.GetLifeformsAsync();

            lifeforms.Should().NotBeNull("it exists");
            lifeforms.Should().HaveCount(3);
            lifeforms.Where(l => l.CommonName == "Dense Blazing Star").Should().HaveCount(1);
            lifeforms.Where(l => l.ScientificName == "Bignonia capreolata").Should().HaveCount(1);
        }
    }
}
