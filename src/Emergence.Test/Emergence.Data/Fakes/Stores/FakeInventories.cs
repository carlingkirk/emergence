using System;
using System.Collections.Generic;
using Emergence.Data.Shared.Stores;

namespace Emergence.Test.Data.Fakes.Stores
{
    public static class FakeInventories
    {
        public static IEnumerable<Inventory> Get() => new List<Inventory>
            {
                new Inventory
                {
                    Id = 0,
                    UserId = new Guid().ToString()
                }
            };

        public static IEnumerable<InventoryItem> GetItems() => new List<InventoryItem>
            {
                new InventoryItem
                {
                    Id = 1,
                    InventoryId = 1,
                    Name = "Liatris spicata seeds",
                    OriginId = 1,
                    ItemType = "Specimen",
                    Quantity = 25,
                    Status = "Available",
                    DateAcquired = new DateTime(2020,03,31),
                    DateCreated = new DateTime(2020,06,15),
                    DateModified = null
                },
                new InventoryItem
                {
                    Id = 2,
                    InventoryId = 1,
                    Name = "Liatris spicata plants",
                    OriginId = 1,
                    ItemType = "Specimen",
                    Quantity = 3,
                    Status = "Available",
                    DateAcquired = new DateTime(2020,03,31),
                    DateCreated = new DateTime(2020,06,15),
                    DateModified = null
                },
                new InventoryItem
                {
                    Id = 3,
                    InventoryId = 1,
                    Name = "Mushroom compost",
                    OriginId = 3,
                    ItemType = "Supply",
                    Quantity = 1,
                    Status = "Wishlist",
                    DateAcquired = new DateTime(2020,03,31),
                    DateCreated = new DateTime(2020,06,15),
                    DateModified = null
                },
            };
    }
}
