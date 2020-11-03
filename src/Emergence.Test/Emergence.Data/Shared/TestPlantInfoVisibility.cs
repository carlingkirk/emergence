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
    public class TestPlantInfoVisibility
    {
        [Fact]
        public void TestCanViewPlantInfos()
        {
            var plantInfos = FakePlantInfos.Get().AsQueryable();
            var visiblePlantInfos = plantInfos.CanViewContent(FakeUsers.GetPublic().AsModel());
            visiblePlantInfos.Should().HaveCount(2);
        }

        [Fact]
        public void TestCannotViewHiddenPlantInfo()
        {
            var plantInfos = new List<PlantInfo>
            {
                new PlantInfo
                {
                    Id = 1,
                    Visibility = Visibility.Hidden
                }
            }.AsQueryable();

            var visiblePlantInfos = plantInfos.CanViewContent(FakeUsers.GetPublic().AsModel());
            visiblePlantInfos.Should().HaveCount(0);
        }

        [Fact]
        public void TestCanViewOwnHiddenPlantInfo()
        {
            var plantInfos = new List<PlantInfo>
            {
                new PlantInfo
                {
                    Id = 1,
                    Visibility = Visibility.Hidden,
                    User = FakeUsers.GetPrivate(),
                    UserId = FakeUsers.GetPrivate().Id
                }
            }.AsQueryable();

            var visiblePlantInfos = plantInfos.CanViewContent(FakeUsers.GetPrivate().AsModel());
            visiblePlantInfos.Should().HaveCount(1);
        }

        [Fact]
        public void TestCannotViewInheritPlantInfoHiddenSpecimens()
        {
            var plantInfos = new List<PlantInfo>
            {
                new PlantInfo
                {
                    Id = 1,
                    Visibility = Visibility.Inherit,
                    User = new User
                    {
                        Contacts = new List<UserContact>(),
                        PlantInfoVisibility = Visibility.Hidden
                    }
                }
            }.AsQueryable();

            var visiblePlantInfos = plantInfos.CanViewContent(FakeUsers.GetPublic().AsModel());
            visiblePlantInfos.Should().HaveCount(0);
        }

        [Fact]
        public void TestCannotViewInheritPlantInfoHiddenUser()
        {
            var plantInfos = new List<PlantInfo>
            {
                new PlantInfo
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

            var visiblePlantInfos = plantInfos.CanViewContent(FakeUsers.GetPublic().AsModel());
            visiblePlantInfos.Should().HaveCount(0);
        }

        [Fact]
        public void TestCannotViewPlantInfoNotInContacts()
        {
            var ownerUser = new User
            {
                Id = 1,
                Contacts = new List<UserContact>()
            };
            var viewingUser = new EmergenceModels.User { Id = 2 };
            var plantInfos = new List<PlantInfo>
            {
                new PlantInfo
                {
                    Id = 1,
                    Visibility = Visibility.Contacts,
                    User = ownerUser
                }
            }.AsQueryable();

            var visiblePlantInfos = plantInfos.CanViewContent(viewingUser);
            visiblePlantInfos.Should().HaveCount(0);
        }

        [Fact]
        public void TestCanViewPlantInfoInContacts()
        {
            var plantInfos = new List<PlantInfo>
            {
                new PlantInfo
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
            var visiblePlantInfos = plantInfos.CanViewContent(user);
            visiblePlantInfos.Should().HaveCount(1);
        }

        [Fact]
        public void TestCanViewPlantInfoInheritInContacts()
        {
            var plantInfos = new List<PlantInfo>
            {
                new PlantInfo
                {
                    Id = 1,
                    Visibility = Visibility.Inherit,
                    User = new User
                    {
                        PlantInfoVisibility = Visibility.Contacts,
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
            var visiblePlantInfos = plantInfos.CanViewContent(user);
            visiblePlantInfos.Should().HaveCount(1);
        }

        [Fact]
        public void TestCanViewPlantInfoInheritNotInContacts()
        {
            var plantInfos = new List<PlantInfo>
            {
                new PlantInfo
                {
                    Id = 1,
                    Visibility = Visibility.Inherit,
                    User = new User
                    {
                        InventoryItemVisibility = Visibility.Contacts,
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
            var visiblePlantInfos = plantInfos.CanViewContent(user);
            visiblePlantInfos.Should().HaveCount(0);
        }

        [Fact]
        public void TestCanViewPlantInfoProfileInheritNotInContacts()
        {
            var plantInfos = new List<PlantInfo>
            {
                new PlantInfo
                {
                    Id = 1,
                    Visibility = Visibility.Inherit,
                    User = new User
                    {
                        ProfileVisibility = Visibility.Contacts,
                        InventoryItemVisibility = Visibility.Inherit,
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
            var visiblePlantInfos = plantInfos.CanViewContent(user);
            visiblePlantInfos.Should().HaveCount(0);
        }

        [Fact]
        public void TestCanViewPlantInfoInheritPublic()
        {
            var plantInfos = new List<PlantInfo>
            {
                new PlantInfo
                {
                    Id = 1,
                    User = new User
                    {
                        ProfileVisibility = Visibility.Public
                    }
                }
            }.AsQueryable();

            var visiblePlantInfos = plantInfos.CanViewContent(FakeUsers.GetPublic().AsModel());
            visiblePlantInfos.Should().HaveCount(1);
        }

        [Fact]
        public void TestCanViewPublicPlantInfoHiddenProfile()
        {
            var plantInfos = new List<PlantInfo>
            {
                new PlantInfo
                {
                    Id = 1,
                    Visibility = Visibility.Public,
                    User = new User
                    {
                        ProfileVisibility = Visibility.Hidden
                    }
                }
            }.AsQueryable();

            var visiblePlantInfos = plantInfos.CanViewContent(FakeUsers.GetPublic().AsModel());
            visiblePlantInfos.Should().HaveCount(1);
        }
    }
}
