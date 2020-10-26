using System.Linq;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Stores;
using Emergence.Service;
using Emergence.Service.Interfaces;
using Emergence.Test.Data.Fakes.Models;
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
            }, FakeUsers.Get().First());

            origins.Results.Should().NotBeNull("it exists");
            origins.Results.Should().HaveCount(1);
            origins.Results.Where(o => o.Visibility == Visibility.Public).Should().HaveCount(1);
        }
    }
}
