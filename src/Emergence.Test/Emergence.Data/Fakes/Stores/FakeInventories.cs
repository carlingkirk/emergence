using System;
using System.Collections.Generic;
using System.Linq;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Stores;

namespace Emergence.Test.Data.Fakes.Stores
{
    public static class FakeInventories
    {
        public static IEnumerable<Inventory> Get() => new List<Inventory>
            {
                new Inventory
                {
                    Id = 1,
                    OwnerId = Helpers.UserId,
                    CreatedBy = Helpers.UserId,
                    DateCreated = Helpers.Today.AddDays(-5)
                }
            };

        public static IEnumerable<InventoryItem> GetItems() => new List<InventoryItem>
            {
                new InventoryItem
                {
                    Id = 1,
                    InventoryId = 1,
                    Inventory = Get().First(i => i.Id == 1),
                    Name = "Liatris spicata seeds",
                    OriginId = 1,
                    ItemType = "Specimen",
                    Quantity = 25,
                    Status = "Available",
                    Visibility = Visibility.Public,
                    CreatedBy = Helpers.UserId,
                    ModifiedBy = null,
                    DateAcquired = new DateTime(2020,03,31),
                    DateCreated = new DateTime(2020,06,15),
                    DateModified = null,
                    User = FakeUsers.GetPublic()
                },
                new InventoryItem
                {
                    Id = 2,
                    InventoryId = 1,
                    Inventory = Get().First(i => i.Id == 1),
                    Name = "Liatris spicata plants",
                    OriginId = 1,
                    ItemType = "Specimen",
                    Quantity = 3,
                    Status = "Available",
                    Visibility = Visibility.Contacts,
                    CreatedBy = Helpers.UserId,
                    ModifiedBy = null,
                    DateAcquired = new DateTime(2020,03,31),
                    DateCreated = new DateTime(2020,06,15),
                    DateModified = null,
                    User = FakeUsers.GetContact()
                },
                new InventoryItem
                {
                    Id = 3,
                    InventoryId = 1,
                    Inventory = Get().First(i => i.Id == 1),
                    Name = "Mushroom compost",
                    OriginId = 3,
                    ItemType = "Supply",
                    Quantity = 1,
                    Status = "Wishlist",
                    Visibility = Visibility.Hidden,
                    CreatedBy = Helpers.UserId,
                    ModifiedBy = null,
                    DateAcquired = null,
                    DateCreated = new DateTime(2020,06,15),
                    DateModified = null,
                    User = FakeUsers.GetPublic()
                },
                new InventoryItem
                {
                    Id = 4,
                    InventoryId = 1,
                    Inventory = Get().First(i => i.Id == 1),
                    Name = "Morning glory seeds",
                    OriginId = 3,
                    ItemType = "Specimen",
                    Quantity = 1,
                    Status = "Wishlist",
                    Visibility = Visibility.Inherit,
                    CreatedBy = Helpers.PrivateUserId,
                    ModifiedBy = null,
                    DateAcquired = null,
                    DateCreated = new DateTime(2020,06,15),
                    DateModified = null,
                    User = FakeUsers.GetPrivate()
                }
            };
    }
}
