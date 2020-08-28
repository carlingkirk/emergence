using System;
using System.Collections.Generic;
using System.Linq;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Test.Data.Fakes.Models
{
    public static class FakeInventories
    {
        public static IEnumerable<Inventory> Get()
        {
            var inventories = new List<Inventory>
            {
                new Inventory
                {
                    Items = new List<InventoryItem>
                    {
                        new InventoryItem
                        {
                            Inventory = new Inventory { InventoryId = 1 },
                            InventoryItemId = 1,
                            DateAcquired = new DateTime(2020,06,26),
                            ItemType = ItemType.Specimen,
                            Name = "Liatris spicata seeds",
                            Quantity = 50,
                            Status = ItemStatus.Available,
                            Origin = new Origin
                            {
                                OriginId = 2,
                                Type = OriginType.Nursery,
                                Name = "Botany Yards",
                                Description = "",
                                Location = new Location
                                {
                                    Latitude = null,
                                    Longitude = null
                                },
                                ParentOrigin = new Origin
                                {
                                    OriginId = 3,
                                    Name = "GNPS Symposium 2020",
                                    Type = OriginType.Event,
                                    Location = FakeLocations.Get().First(),
                                    Uri = new Uri("https://gnps.org/2020-georgia-native-plant-society-annual-symposium/")
                                }
                            }
                        }
                    }
                }
            };

            return inventories;
        }
    }
}
