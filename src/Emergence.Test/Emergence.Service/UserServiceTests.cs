using System;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;
using Emergence.Service;
using Emergence.Test.Data.Fakes.Stores;
using Emergence.Test.Mocks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Emergence.Test.Emergence.Service
{
    public class UserServiceTests
    {
        private readonly Mock<IRepository<User>> _mockUserRepository;

        public UserServiceTests()
        {
            _mockUserRepository = RepositoryMocks.GetStandardMockUserRepository();
        }

        [Fact]
        public async Task TestGetUserAsync()
        {
            var photoService = ServiceMocks.GetStandardMockPhotoService();
            var locationService = ServiceMocks.GetStandardMockLocationService();
            var nameRepository = new Mock<IRepository<DisplayName>>();
            var cacheService = ServiceMocks.GetStandardMockCacheService();
            var userService = new UserService(_mockUserRepository.Object, nameRepository.Object, photoService.Object, locationService.Object, cacheService.Object);
            var user = await userService.GetUserAsync(1, FakeUsers.GetPublic().AsModel());

            user.Should().NotBeNull("it exists");
            user.DateCreated.Should().BeBefore(DateTime.UtcNow);
            user.FirstName.Should().Be("Daria");
            user.Location.StateOrProvince.Should().Be("GA");
            user.ProfileVisibility.Should().Be(Visibility.Public);
            user.ActivityVisibility.Should().Be(Visibility.Public);
            user.OriginVisibility.Should().Be(Visibility.Public);
            user.PlantInfoVisibility.Should().Be(Visibility.Public);
            user.InventoryItemVisibility.Should().Be(Visibility.Public);
        }
    }
}
