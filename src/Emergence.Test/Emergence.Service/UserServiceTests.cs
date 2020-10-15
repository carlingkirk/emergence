using System;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Shared.Stores;
using Emergence.Service;
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
            var userService = new UserService(_mockUserRepository.Object, photoService.Object, locationService.Object);
            var user = await userService.GetUserAsync(1);

            user.Should().NotBeNull("it exists");
            user.DateCreated.Should().BeBefore(DateTime.UtcNow);
            user.FirstName.Should().Be("Daria");
            user.Location.StateOrProvince.Should().Be("GA");
            user.Photo.OriginalUri.Should().Be("https://blobs.com/photos/" + user.Photo.BlobPath + "/1.png");
        }
    }
}