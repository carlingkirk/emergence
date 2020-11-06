using System.Collections.Generic;
using System.Linq;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;
using Emergence.Test.Data.Fakes.Stores;
using FluentAssertions;
using Xunit;

namespace Emergence.Test.Emergence.Data.Shared
{
    public class TestUserVisibility
    {
        [Fact]
        public void TestCanViewUser()
        {
            var users = FakeUsers.Get().AsQueryable();
            var visibleUsers = users.CanViewUsers(FakeUsers.GetPublic().AsModel());
            visibleUsers.Should().HaveCount(2);
        }

        [Fact]
        public void TestCannotViewPrivateUser()
        {
            var users = FakeUsers.Get().AsQueryable().Where(u => u.ProfileVisibility == Visibility.Hidden);
            var visibleUsers = users.CanViewUsers(FakeUsers.GetPublic().AsModel());
            visibleUsers.Should().HaveCount(0);
        }

        [Fact]
        public void TestCanViewContactUser()
        {
            var users = new List<User> { FakeUsers.GetContact() }.AsQueryable();
            var visibleUsers = users.CanViewUsers(FakeUsers.GetPublic().AsModel());
            visibleUsers.Should().HaveCount(1);
        }

        [Fact]
        public void TestCannotViewContactUser()
        {
            var users = new List<User> { FakeUsers.GetContact() }.AsQueryable();
            var visibleUsers = users.CanViewUsers(FakeUsers.GetPrivate().AsModel());
            visibleUsers.Should().HaveCount(0);
        }
    }
}
