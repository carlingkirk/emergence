using System.Collections.Generic;
using System.Linq;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;
using Emergence.Test.Data.Fakes.Stores;
using FluentAssertions;
using Xunit;
using EmergenceModels = Emergence.Data.Shared.Models;

namespace Emergence.Test.Data.Shared
{
    public class TestActivityVisibility
    {
        [Fact]
        public void TestCanViewActivities()
        {
            var activities = FakeActivities.Get().AsQueryable();
            var visibleActivities = activities.CanViewContent(FakeUsers.GetPublic().AsModel());
            visibleActivities.Should().HaveCount(3);
        }

        [Fact]
        public void TestCannotViewHiddenActivity()
        {
            var activities = new List<Activity>
            {
                new Activity
                {
                    Id = 1,
                    Visibility = Visibility.Hidden
                }
            }.AsQueryable();

            var visibleActivities = activities.CanViewContent(FakeUsers.GetPublic().AsModel());
            visibleActivities.Should().HaveCount(0);
        }

        [Fact]
        public void TestCanViewOwnHiddenActivity()
        {
            var activities = new List<Activity>
            {
                new Activity
                {
                    Id = 1,
                    Visibility = Visibility.Hidden,
                    User = FakeUsers.GetPrivate(),
                    UserId = FakeUsers.GetPrivate().Id
                }
            }.AsQueryable();

            var visibleActivities = activities.CanViewContent(FakeUsers.GetPrivate().AsModel());
            visibleActivities.Should().HaveCount(1);
        }

        [Fact]
        public void TestCannotViewInheritActivityHiddenActivities()
        {
            var activities = new List<Activity>
            {
                new Activity
                {
                    Id = 1,
                    Visibility = Visibility.Inherit,
                    User = new User
                    {
                        Contacts = new List<UserContact>(),
                        ActivityVisibility = Visibility.Hidden
                    }
                }
            }.AsQueryable();

            var visibleActivities = activities.CanViewContent(FakeUsers.GetPublic().AsModel());
            visibleActivities.Should().HaveCount(0);
        }

        [Fact]
        public void TestCannotViewInheritActivityHiddenUser()
        {
            var activities = new List<Activity>
            {
                new Activity
                {
                    Id = 1,
                    Visibility = Visibility.Inherit,
                    User = new User
                    {
                        Contacts = new List<UserContact>(),
                        ActivityVisibility = Visibility.Inherit,
                        ProfileVisibility = Visibility.Hidden
                    }
                }
            }.AsQueryable();

            var visibleActivities = activities.CanViewContent(FakeUsers.GetPublic().AsModel());
            visibleActivities.Should().HaveCount(0);
        }

        [Fact]
        public void TestCannotViewActivityNotInContacts()
        {
            var ownerUser = new User
            {
                Id = 1,
                Contacts = new List<UserContact>()
            };
            var viewingUser = new EmergenceModels.User { Id = 2 };
            var activities = new List<Activity>
            {
                new Activity
                {
                    Id = 1,
                    Visibility = Visibility.Contacts,
                    User = ownerUser
                }
            }.AsQueryable();

            var visibleActivities = activities.CanViewContent(viewingUser);
            visibleActivities.Should().HaveCount(0);
        }

        [Fact]
        public void TestCanViewActivityInContacts()
        {
            var activities = new List<Activity>
            {
                new Activity
                {
                    Id = 1,
                    Visibility = Visibility.Contacts,
                    User = new User
                    {
                        Contacts = new List<UserContact>()
                        {
                            new UserContact
                            {
                                Id = 1,
                                UserId = 1,
                                ContactUserId = 2
                            }
                        }
                    }
                }
            }.AsQueryable();

            var user = new EmergenceModels.User { Id = 2 };
            var visibleActivities = activities.CanViewContent(user);
            visibleActivities.Should().HaveCount(1);
        }

        [Fact]
        public void TestCanViewActivityInheritInContacts()
        {
            var activities = new List<Activity>
            {
                new Activity
                {
                    Id = 1,
                    Visibility = Visibility.Inherit,
                    User = new User
                    {
                        ActivityVisibility = Visibility.Contacts,
                        Contacts = new List<UserContact>()
                        {
                            new UserContact
                            {
                                Id = 1,
                                UserId = 1,
                                ContactUserId = 2
                            }
                        }
                    }
                }
            }.AsQueryable();

            var user = new EmergenceModels.User { Id = 2 };
            var visibleActivities = activities.CanViewContent(user);
            visibleActivities.Should().HaveCount(1);
        }

        [Fact]
        public void TestCanViewActivityInheritPublic()
        {
            var activities = new List<Activity>
            {
                new Activity
                {
                    Id = 1,
                    Visibility = Visibility.Inherit,
                    User = new User
                    {
                        ActivityVisibility = Visibility.Public
                    }
                }
            }.AsQueryable();

            var user = new EmergenceModels.User { Id = 2 };
            var visibleActivities = activities.CanViewContent(user);
            visibleActivities.Should().HaveCount(1);
        }
    }
}
