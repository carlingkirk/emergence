using System.Linq;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;
using Emergence.Service;
using Emergence.Test.Data.Fakes.Stores;
using Emergence.Test.Mocks;
using FluentAssertions;
using Moq;
using Xunit;
using Models = Emergence.Data.Shared.Models;

namespace Emergence.Test.Emergence.Service
{
    public class UserContactServiceTests
    {
        private readonly Mock<IRepository<UserContact>> _mockUserContactRepository;
        private readonly Mock<IRepository<UserContactRequest>> _mockUserContactRequestRepository;

        public UserContactServiceTests()
        {
            _mockUserContactRepository = RepositoryMocks.GetStandardMockUserContactRepository();
            _mockUserContactRequestRepository = RepositoryMocks.GetStandardMockUserContactRequestRepository();
        }

        [Fact]
        public async Task TestGetUserContactsAsync()
        {
            var userContactService = new UserContactService(_mockUserContactRepository.Object, _mockUserContactRequestRepository.Object);
            var userContacts = await userContactService.GetUserContactsAsync(1);

            userContacts.Should().HaveCount(3);
            userContacts.Count(uc => uc.ContactUserId == 2).Should().Be(1);
        }

        [Fact]
        public async Task TestGetUserContactRequestsAsync()
        {
            var userContactService = new UserContactService(_mockUserContactRepository.Object, _mockUserContactRequestRepository.Object);
            var userContactRequests = await userContactService.GetUserContactRequestsAsync(1);

            userContactRequests.Should().HaveCount(3);
            userContactRequests.Count(uc => uc.ContactUserId == 5).Should().Be(1);
        }

        [Fact]
        public async Task TestGetUserContactStatusAsync()
        {
            var userContactService = new UserContactService(_mockUserContactRepository.Object, _mockUserContactRequestRepository.Object);
            var requestor = FakeUsers.GetPublic().AsModel();
            var user = FakeUsers.GetContact().AsModel();
            var userContactStatus = await userContactService.GetUserContactStatusAsync(requestor, user);

            userContactStatus.Contacts.Count(c => c.UserId == requestor.Id).Should().Be(1);
        }

        [Fact]
        public async Task TestAddUserContactAsync()
        {
            var today = Helpers.Today;
            var userContactService = new UserContactService(_mockUserContactRepository.Object, _mockUserContactRequestRepository.Object);
            var userContact = await userContactService.AddUserContactAsync(new Models.UserContactRequest
            {
                UserId = 1,
                ContactUserId = 5,
                DateRequested = today
            });

            userContact.Id.Should().BeGreaterThan(0);
            userContact.DateRequested.Should().Be(today);
            userContact.DateAccepted.Should().BeAfter(today);
        }

        [Fact]
        public async Task TestAddUserContactRequestAsync()
        {
            var today = Helpers.Today;
            var userContactService = new UserContactService(_mockUserContactRepository.Object, _mockUserContactRequestRepository.Object);
            var userContactRequest = await userContactService.AddUserContactRequestAsync(new Models.UserContactRequest
            {
                UserId = 1,
                ContactUserId = 5,
                DateRequested = today
            });

            userContactRequest.Id.Should().BeGreaterThan(0);
            userContactRequest.DateRequested.Should().Be(today);
        }

        [Fact]
        public async Task TestRemoveUserContactAsync()
        {
            var userContactService = new UserContactService(_mockUserContactRepository.Object, _mockUserContactRequestRepository.Object);
            var result = await userContactService.RemoveUserContactAsync(1);
            result.Should().BeTrue();
        }

        [Fact]
        public async Task TestRemoveUserContactRequestAsync()
        {
            var userContactService = new UserContactService(_mockUserContactRepository.Object, _mockUserContactRequestRepository.Object);
            var result = await userContactService.RemoveUserContactRequestAsync(4);
            result.Should().BeTrue();
        }
    }
}
