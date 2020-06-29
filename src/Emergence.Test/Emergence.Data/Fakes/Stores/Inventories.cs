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
                    UserId = 0
                }
            };

        public static IEnumerable<InventoryItem> GetItems() => new List<InventoryItem>
            {
                new InventoryItem
                {
                    Id = 0,
                    InventoryId = 0,
                    Name = "Liatris Spicata seeds",
                    OriginId = 0,
                    ItemType = "Specimen",
                    Quantity = 25,
                    Status = "Available",
                    DateAcquired = new DateTime(2020,03,31),
                    DateCreated = new DateTime(2020,06,15),
                    DateModified = null
                },
                new InventoryItem
                {
                    Id = 1,
                    InventoryId = 0,
                    Name = "Liatris spicata plants",
                    OriginId = 0,
                    ItemType = "Specimen",
                    Quantity = 3,
                    Status = "Available",
                    DateAcquired = new DateTime(2020,03,31),
                    DateCreated = new DateTime(2020,06,15),
                    DateModified = null
                },
                new InventoryItem
                {
                    Id = 2,
                    InventoryId = 0,
                    Name = "Mushroom compost",
                    OriginId = 0,
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
