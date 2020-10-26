using System.Linq;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Stores;
using Emergence.Service;
using Emergence.Service.Interfaces;
using Emergence.Test.Mocks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Emergence.Test.API.Services
{
    public class OriginServiceTests : TestBase
    {
        private readonly Mock<IRepository<Origin>> _mockOriginRepository;
        private readonly Mock<ILocationService> _mockLocationService;
        public OriginServiceTests()
        {
            _mockOriginRepository = RepositoryMocks.GetStandardMockOriginRepository();
            _mockLocationService = ServiceMocks.GetStandardMockLocationService();
        }

        [Fact]
        public async Task TestGetOriginAsync()
        {
            var originService = new OriginService(_mockOriginRepository.Object, _mockLocationService.Object);
            var origin = await originService.GetOriginAsync(0);

            origin.Should().NotBeNull("it exists");
        }

        [Fact]
        public async Task TestGetOriginsAsync()
        {
            var originService = new OriginService(_mockOriginRepository.Object, _mockLocationService.Object);
            var origins = await originService.GetOriginsAsync();

            origins.Should().NotBeNull("it exists");
            origins.Should().HaveCount(3);
            origins.Where(o => o.Type == OriginType.Website).Should().HaveCount(1);
            origins.Where(o => o.Uri == null).Should().HaveCount(1);
            origins.Where(o => o.Name == "Botany Yards").Should().HaveCount(1);
            origins.Where(o => o.Visibility == Visibility.Contacts).Should().HaveCount(1);
        }

        [Fact]
        public async Task TestFindOrigins()
        {
            var mockOriginRepository = RepositoryMocks.GetStandardMockOriginRepository(Data.Fakes.Stores.FakeOrigins.Get().Where(o => o.Name == "Botany Yards"));

            var originService = new OriginService(mockOriginRepository.Object, _mockLocationService.Object);
            var origins = await originService.FindOrigins(new FindParams
            {
                SearchText = "Botany",
                Skip = 0,
                Take = 10,
                SortBy = "",
                SortDirection = SortDirection.None
            }, Helpers.UserId);

            origins.Results.Should().NotBeNull("it exists");
            origins.Results.Should().HaveCount(1);
            origins.Results.Where(o => o.Visibility == Visibility.Public).Should().HaveCount(1);
        }
    }
}
