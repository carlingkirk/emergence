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
    public class TestInventoryItemVisibility
    {
        [Fact]
        public void TestCanViewInventoryItems()
        {
            var inventoryItems = FakeInventories.GetItems().AsQueryable();
            var visibleInventoryItems = inventoryItems.CanViewContent(FakeUsers.GetPublic().AsModel());
            visibleInventoryItems.Should().HaveCount(2);
        }

        [Fact]
        public void TestCannotViewHiddenInventoryItem()
        {
            var inventoryItems = new List<InventoryItem>
            {
                new InventoryItem
                {
                    Id = 1,
                    Visibility = Visibility.Hidden
                }
            }.AsQueryable();

            var visibleInventoryItems = inventoryItems.CanViewContent(FakeUsers.GetPublic().AsModel());

            visibleInventoryItems.Should().HaveCount(0);
        }

        [Fact]
        public void TestCannotViewInheritInventoryItemHiddenSpecimens()
        {
            var inventoryItems = new List<InventoryItem>
            {
                new InventoryItem
                {
                    Id = 1,
                    Visibility = Visibility.Inherit,
                    User = new User
                    {
                        Contacts = new List<UserContact>(),
                        InventoryItemVisibility = Visibility.Hidden
                    }
                }
            }.AsQueryable();

            var visibleInventoryItems = inventoryItems.CanViewContent(FakeUsers.GetPublic().AsModel());
            visibleInventoryItems.Should().HaveCount(0);
        }

        [Fact]
        public void TestCannotViewInheritInventoryItemHiddenUser()
        {
            var inventoryItems = new List<InventoryItem>
            {
                new InventoryItem
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

            var visibleInventoryItems = inventoryItems.CanViewContent(FakeUsers.GetPublic().AsModel());
            visibleInventoryItems.Should().HaveCount(0);
        }

        [Fact]
        public void TestCannotViewInventoryItemNotInContacts()
        {
            var ownerUser = new User
            {
                Id = 1,
                Contacts = new List<UserContact>()
            };
            var viewingUser = new EmergenceModels.User { Id = 2 };
            var inventoryItems = new List<InventoryItem>
            {
                new InventoryItem
                {
                    Id = 1,
                    Visibility = Visibility.Contacts,
                    User = ownerUser
                }
            }.AsQueryable();

            var visibleInventoryItems = inventoryItems.CanViewContent(viewingUser);
            visibleInventoryItems.Should().HaveCount(0);
        }

        [Fact]
        public void TestCanViewInventoryItemInContacts()
        {
            var inventoryItems = new List<InventoryItem>
            {
                new InventoryItem
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
            var visibleInventoryItems = inventoryItems.CanViewContent(user);
            visibleInventoryItems.Should().HaveCount(1);
        }

        [Fact]
        public void TestCanViewInventoryItemInheritInContacts()
        {
            var inventoryItems = new List<InventoryItem>
            {
                new InventoryItem
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
            }.AsQueryable();

            var user = new EmergenceModels.User { Id = 2 };
            var visibleInventoryItems = inventoryItems.CanViewContent(user);
            visibleInventoryItems.Should().HaveCount(1);
        }
    }
}
