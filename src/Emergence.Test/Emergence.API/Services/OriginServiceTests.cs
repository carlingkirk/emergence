using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Emergence.API.Services;
using Emergence.Data;
using Emergence.Data.Shared.Stores;
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
            _mockOriginRepository.Setup(p => p.GetSomeAsync(It.IsAny<Expression<Func<Origin, bool>>>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<bool>()))
                .Returns(Data.Fakes.Stores.FakeOrigins.Get().Where(o => o.Name == "Botany Yards").ToAsyncEnumerable());

            var originService = new OriginService(_mockOriginRepository.Object);
            var specimens = await originService.FindOrigins("Botany");

            specimens.Should().NotBeNull("it exists");
            specimens.Should().HaveCount(1);
        }
    }
}
