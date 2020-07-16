using System.Linq;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Shared.Stores;
using Emergence.Service;
using Emergence.Test.Mocks;
using FluentAssertions;
using Moq;
using Xunit;
using Models = Emergence.Data.Shared.Models;

namespace Emergence.Test.Emergence.API.Services
{
    public class ActivityServiceTests
    {
        private readonly Mock<IRepository<Activity>> _mockActivityRepository;

        public ActivityServiceTests()
        {
            _mockActivityRepository = RepositoryMocks.GetStandardMockActivityRepository();
        }

        [Fact]
        public async Task TestGetActivityAsync()
        {
            var specimenService = ServiceMocks.GetStandardMockSpecimenService();
            var inventoryService = ServiceMocks.GetStandardMockInventoryService();
            var activityService = new ActivityService(_mockActivityRepository.Object, specimenService.Object, inventoryService.Object);
            var activity = await activityService.GetActivityAsync(1);

            activity.Should().NotBeNull("it exists");
        }

        [Fact]
        public async Task TestGetActivitiesAsync()
        {
            var specimenService = ServiceMocks.GetStandardMockSpecimenService();
            var inventoryService = ServiceMocks.GetStandardMockInventoryService();
            var activityService = new ActivityService(_mockActivityRepository.Object, specimenService.Object, inventoryService.Object);
            var activities = await activityService.GetActivitiesAsync();

            activities.Should().NotBeNull("it exists");
            activities.Should().HaveCount(3);
            activities.Where(a => a.Specimen.InventoryItem.Name == "Liatris spicata Seeds").Should().HaveCount(3);
            activities.Where(a => a.ActivityType == Models.ActivityType.Stratification).Should().HaveCount(1);
            activities.Where(a => a.DateOccured != null && a.DateOccured.Value.Month == 3).Should().HaveCount(1);
        }
    }
}
