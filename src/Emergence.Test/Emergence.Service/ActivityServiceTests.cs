using System.Linq;
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
            var activityService = new ActivityService(_mockActivityRepository.Object);
            var activity = await activityService.GetActivityAsync(1, FakeUsers.GetPublic().AsModel());

            activity.Should().NotBeNull("it exists");
        }

        [Fact]
        public async Task TestFindActivitiesOwnerUser()
        {
            var activityService = new ActivityService(_mockActivityRepository.Object);
            var activities = await activityService.FindActivities(new FindParams
            {
                SearchText = "Liatris spicata",
                Skip = 0,
                Take = 10,
                SortBy = "",
                SortDirection = SortDirection.None
            }, FakeUsers.GetPublic().AsModel());

            activities.Should().NotBeNull("it exists");
            activities.Results.Should().HaveCount(3);
            activities.Count.Should().Be(3);
            activities.Results.Where(a => a.Specimen.InventoryItem.Name == "Liatris spicata seeds").Should().HaveCount(3);
            activities.Results.Where(a => a.ActivityType == ActivityType.Stratification).Should().HaveCount(1);
            activities.Results.Where(a => a.DateOccurred != null).Should().HaveCount(3);
            activities.Results.Where(a => a.DateScheduled != null).Should().HaveCount(2);
            activities.Results.Where(a => a.DateCreated != null).Should().HaveCount(3);
            activities.Results.Where(a => a.DateModified != null).Should().HaveCount(2);
            activities.Results.Where(a => a.Visibility == Visibility.Public).Should().HaveCount(1);
        }

        [Fact]
        public async Task TestFindActivitiesPrivateUser()
        {
            var activityService = new ActivityService(_mockActivityRepository.Object);
            var activities = await activityService.FindActivities(new FindParams
            {
                SearchText = "Liatris spicata",
                Skip = 0,
                Take = 10,
                SortBy = "",
                SortDirection = SortDirection.None
            }, FakeUsers.GetPrivate().AsModel());

            activities.Should().NotBeNull("it exists");
            activities.Results.Should().HaveCount(2, "Private user is not in activity owner's contacts");
            activities.Count.Should().Be(2);
            activities.Results.Where(a => a.Specimen.InventoryItem.Name == "Liatris spicata seeds").Should().HaveCount(2);
            activities.Results.Where(a => a.ActivityType == ActivityType.Stratification).Should().HaveCount(1);
            activities.Results.Where(a => a.DateOccurred != null).Should().HaveCount(2);
            activities.Results.Where(a => a.DateScheduled != null).Should().HaveCount(2);
            activities.Results.Where(a => a.DateCreated != null).Should().HaveCount(2);
            activities.Results.Where(a => a.DateModified != null).Should().HaveCount(1);
            activities.Results.Where(a => a.Visibility == Visibility.Public).Should().HaveCount(1);
        }
    }
}
