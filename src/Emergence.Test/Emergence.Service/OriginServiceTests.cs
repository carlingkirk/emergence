using System.Linq;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Stores;
using Emergence.Service;
using Emergence.Test.Mocks;
using FluentAssertions;
using Moq;
using Xunit;
using Models = Emergence.Data.Shared.Models;

namespace Emergence.Test.API.Services
{
    public class OriginServiceTests : TestBase
    {
        private readonly Mock<IRepository<Origin>> _mockOriginRepository;
        public OriginServiceTests()
        {
            _mockOriginRepository = RepositoryMocks.GetStandardMockOriginRepository();
        }

        [Fact]
        public async Task TestGetOriginAsync()
        {
            var originService = new OriginService(_mockOriginRepository.Object);
            var origin = await originService.GetOriginAsync(0);

            origin.Should().NotBeNull("it exists");
        }

        [Fact]
        public async Task TestGetOriginsAsync()
        {
            var originService = new OriginService(_mockOriginRepository.Object);
            var origins = await originService.GetOriginsAsync();

            origins.Should().NotBeNull("it exists");
            origins.Should().HaveCount(3);
            origins.Where(o => o.Type == Models.OriginType.Website).Should().HaveCount(1);
            origins.Where(o => o.Uri == null).Should().HaveCount(1);
            origins.Where(o => o.Name == "Botany Yards").Should().HaveCount(1);
        }

        [Fact]
        public async Task TestFindOrigins()
        {
            var mockOriginRepository = RepositoryMocks.GetStandardMockOriginRepository(Data.Fakes.Stores.FakeOrigins.Get().Where(o => o.Name == "Botany Yards"));

            var originService = new OriginService(mockOriginRepository.Object);
            var specimens = await originService.FindOrigins("Botany", "me", 0, 10, "", SortDirection.None);

            specimens.Results.Should().NotBeNull("it exists");
            specimens.Results.Should().HaveCount(1);
        }
    }
}
