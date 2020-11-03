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
    public class TestOriginVisibility
    {
        [Fact]
        public void TestCanViewOrigins()
        {
            var origins = FakeOrigins.Get().AsQueryable();
            var visibleOrigins = origins.CanViewContent(FakeUsers.GetPublic().AsModel());
            visibleOrigins.Should().HaveCount(1);
        }

        [Fact]
        public void TestCannotViewHiddenOrigin()
        {
            var origins = new List<Origin>
            {
                new Origin
                {
                    Id = 1,
                    Visibility = Visibility.Hidden
                }
            }.AsQueryable();

            var visibleOrigins = origins.CanViewContent(FakeUsers.GetPublic().AsModel());

            visibleOrigins.Should().HaveCount(0);
        }

        [Fact]
        public void TestCanViewOwnHiddenOrigin()
        {
            var origins = new List<Origin>
            {
                new Origin
                {
                    Id = 1,
                    Visibility = Visibility.Hidden,
                    User = FakeUsers.GetPrivate(),
                    UserId = FakeUsers.GetPrivate().Id
                }
            }.AsQueryable();

            var visibleOrigins = origins.CanViewContent(FakeUsers.GetPrivate().AsModel());
            visibleOrigins.Should().HaveCount(1);
        }

        [Fact]
        public void TestCannotViewInheritOriginHiddenSpecimens()
        {
            var origins = new List<Origin>
            {
                new Origin
                {
                    Id = 1,
                    Visibility = Visibility.Inherit,
                    User = new User
                    {
                        Contacts = new List<UserContact>(),
                        OriginVisibility = Visibility.Hidden
                    }
                }
            }.AsQueryable();

            var visibleOrigins = origins.CanViewContent(FakeUsers.GetPublic().AsModel());
            visibleOrigins.Should().HaveCount(0);
        }

        [Fact]
        public void TestCannotViewInheritOriginHiddenUser()
        {
            var origins = new List<Origin>
            {
                new Origin
                {
                    Id = 1,
                    Visibility = Visibility.Inherit,
                    User = new User
                    {
                        Contacts = new List<UserContact>(),
                        ProfileVisibility = Visibility.Hidden
                    }
                }
            }.AsQueryable();

            var visibleOrigins = origins.CanViewContent(FakeUsers.GetPublic().AsModel());
            visibleOrigins.Should().HaveCount(0);
        }

        [Fact]
        public void TestCannotViewOriginNotInContacts()
        {
            var ownerUser = new User
            {
                Id = 1,
                Contacts = new List<UserContact>()
            };
            var viewingUser = new EmergenceModels.User { Id = 2 };
            var origins = new List<Origin>
            {
                new Origin
                {
                    Id = 1,
                    Visibility = Visibility.Contacts,
                    User = ownerUser
                }
            }.AsQueryable();

            var visibleOrigins = origins.CanViewContent(viewingUser);
            visibleOrigins.Should().HaveCount(0);
        }

        [Fact]
        public void TestCanViewOriginInContacts()
        {
            var origins = new List<Origin>
            {
                new Origin
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
            var visibleOrigins = origins.CanViewContent(user);
            visibleOrigins.Should().HaveCount(1);
        }

        [Fact]
        public void TestCanViewOriginInheritInContacts()
        {
            var origins = new List<Origin>
            {
                new Origin
                {
                    Id = 1,
                    Visibility = Visibility.Inherit,
                    User = new User
                    {
                        OriginVisibility = Visibility.Contacts,
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
            var visibleOrigins = origins.CanViewContent(user);
            visibleOrigins.Should().HaveCount(1);
        }

        [Fact]
        public void TestCanViewOriginInheritNotInContacts()
        {
            var origins = new List<Origin>
            {
                new Origin
                {
                    Id = 1,
                    Visibility = Visibility.Inherit,
                    User = new User
                    {
                        OriginVisibility = Visibility.Contacts,
                        Contacts = new List<UserContact>()
                        {
                            new UserContact
                            {
                                Id = 1,
                                UserId = 1,
                                ContactUserId = 5
                            }
                        }
                    }
                }
            }.AsQueryable();

            var user = new EmergenceModels.User { Id = 2 };
            var visibleOrigins = origins.CanViewContent(user);
            visibleOrigins.Should().HaveCount(0);
        }

        [Fact]
        public void TestCanViewOriginProfileInheritNotInContacts()
        {
            var origins = new List<Origin>
            {
                new Origin
                {
                    Id = 1,
                    Visibility = Visibility.Inherit,
                    User = new User
                    {
                        ProfileVisibility = Visibility.Contacts,
                        OriginVisibility = Visibility.Inherit,
                        Contacts = new List<UserContact>()
                        {
                            new UserContact
                            {
                                Id = 1,
                                UserId = 1,
                                ContactUserId = 5
                            }
                        }
                    }
                }
            }.AsQueryable();

            var user = new EmergenceModels.User { Id = 2 };
            var visibleOrigins = origins.CanViewContent(user);
            visibleOrigins.Should().HaveCount(0);
        }

        [Fact]
        public void TestCanViewOriginInheritPublic()
        {
            var origins = new List<Origin>
            {
                new Origin
                {
                    Id = 1,
                    User = new User
                    {
                        ProfileVisibility = Visibility.Public
                    }
                }
            }.AsQueryable();

            var visibleOrigins = origins.CanViewContent(FakeUsers.GetPublic().AsModel());
            visibleOrigins.Should().HaveCount(1);
        }

        [Fact]
        public void TestCanViewPublicOriginHiddenProfile()
        {
            var origins = new List<Origin>
            {
                new Origin
                {
                    Id = 1,
                    Visibility = Visibility.Public,
                    User = new User
                    {
                        ProfileVisibility = Visibility.Hidden
                    }
                }
            }.AsQueryable();

            var visibleOrigins = origins.CanViewContent(FakeUsers.GetPublic().AsModel());
            visibleOrigins.Should().HaveCount(1);
        }
    }
}
