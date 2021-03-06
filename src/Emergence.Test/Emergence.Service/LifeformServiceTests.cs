using System.Linq;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Shared.Search.Models;
using Emergence.Service;
using Emergence.Service.Search;
using Emergence.Test.Mocks;
using FluentAssertions;
using Moq;
using Xunit;
using Models = Emergence.Data.Shared.Models;
using Stores = Emergence.Data.Shared.Stores;

namespace Emergence.Test.API.Services
{
    public class LifeformServiceTests
    {
        private readonly Mock<IRepository<Stores.Lifeform>> _mockLifeformRepository;
        private readonly Mock<IIndex<Lifeform, Models.Lifeform>> _mockLifeformIndex;

        public LifeformServiceTests()
        {
            _mockLifeformRepository = RepositoryMocks.GetStandardMockLifeformRepository();
            _mockLifeformIndex = SearchMocks.GetStandardMockLifeformIndex();
        }

        [Fact]
        public async Task TestGetLifeformAsync()
        {
            var lifeformsService = new LifeformService(_mockLifeformRepository.Object, _mockLifeformIndex.Object);
            var lifeforms = await lifeformsService.GetLifeformAsync(1);

            lifeforms.Should().NotBeNull("it exists");
        }

        [Fact]
        public async Task TestGetLifeformsAsync()
        {
            var lifeformService = new LifeformService(_mockLifeformRepository.Object, _mockLifeformIndex.Object);
            var lifeforms = await lifeformService.GetLifeformsAsync();

            lifeforms.Should().NotBeNull("it exists");
            lifeforms.Should().HaveCount(4);
            lifeforms.Where(l => l.CommonName == "Dense Blazing Star").Should().HaveCount(1);
            lifeforms.Where(l => l.ScientificName == "Bignonia capreolata").Should().HaveCount(1);
        }
    }
}
