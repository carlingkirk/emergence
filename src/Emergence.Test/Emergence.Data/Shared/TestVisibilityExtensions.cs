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
    public class TestVisibilityExtensions
    {
        [Fact]
        public void TestCanViewSpecimen()
        {
            var specimens = FakeSpecimens.Get().AsQueryable<Specimen>();
            var visibleSpecimens = specimens.CanViewContent(FakeUsers.Get().First().AsModel());
            visibleSpecimens.Should().HaveCount(1);
        }

        [Fact]
        public void TestCannotViewHiddenSpecimen()
        {
            var specimens = new List<Specimen>
            {
                new Specimen
                {
                    Id = 1,
                    InventoryItem = new InventoryItem
                    {
                        Id = 1,
                        Visibility = Visibility.Hidden
                    }
                }
            }.AsQueryable();

            var visibleSpecimens = specimens.CanViewContent(FakeUsers.Get().First().AsModel());
            visibleSpecimens.Should().HaveCount(0);
        }

        [Fact]
        public void TestCannotViewInheritSpecimenHiddenUser()
        {
            var specimens = new List<Specimen>
            {
                new Specimen
                {
                    Id = 1,
                    InventoryItem = new InventoryItem
                    {
                        Id = 1,
                        Visibility = Visibility.Inherit,
                        User = new User
                        {
                            Contacts = new List<UserContact>(),
                            InventoryItemVisibility = Visibility.Hidden
                        }
                    }
                }
            }.AsQueryable();

            var visibleSpecimens = specimens.CanViewContent(FakeUsers.Get().First().AsModel());
            visibleSpecimens.Should().HaveCount(0);
        }

        [Fact]
        public void TestCannotViewSpecimenNotInContacts()
        {
            var specimens = new List<Specimen>
            {
                new Specimen
                {
                    Id = 1,
                    InventoryItem = new InventoryItem
                    {
                        Id = 1,
                        Visibility = Visibility.Contacts,
                        User = new User
                        {
                            Contacts = new List<UserContact>()
                        }
                    }
                }
            }.AsQueryable();

            var user = new EmergenceModels.User { Id = 2 };
            var visibleSpecimens = specimens.CanViewContent(user);
            visibleSpecimens.Should().HaveCount(0);
        }

        [Fact]
        public void TestCanViewSpecimenInContacts()
        {
            var specimens = new List<Specimen>
            {
                new Specimen
                {
                    Id = 1,
                    InventoryItem = new InventoryItem
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
                }
            }.AsQueryable();

            var user = new EmergenceModels.User { Id = 2 };
            var visibleSpecimens = specimens.CanViewContent(user);
            visibleSpecimens.Should().HaveCount(1);
        }

        [Fact]
        public void TestCanViewSpecimenInheritInContacts()
        {
            var specimens = new List<Specimen>
            {
                new Specimen
                {
                    Id = 1,
                    InventoryItem = new InventoryItem
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
                                    ContactUserId = 2
                                }
                            }
                        }
                    }
                }
            }.AsQueryable();

            var user = new EmergenceModels.User { Id = 2 };
            var visibleSpecimens = specimens.CanViewContent(user);
            visibleSpecimens.Should().HaveCount(1);
        }
    }
}
