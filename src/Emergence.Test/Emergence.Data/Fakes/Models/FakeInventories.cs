using System;
using System.Collections.Generic;
using Emergence.Data.Shared.Models;

namespace Emergence.Test.Data.Fakes.Models
{
    public static class FakeInventories
    {
        public static IEnumerable<Inventory> Inventories()
        {
            var inventories = new List<Inventory>
            {
                new Inventory
                {
                    Items = new List<InventoryItem>
                    {
                        new InventoryItem
                        {
                            InventoryId = 0,
                            InventoryItemId = 0,
                            DateAcquired = new DateTime(2020,06,26),
                            ItemType = ItemType.Specimen,
                            Name = "Liatris Spicata Seeds",
                            Quantity = 50,
                            Status = Status.Available,
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
                                    Location = new Location
                                    {
                                        AddressLine1 = "100 University Parkway",
                                        AddressLine2 = "Charles H. Jones Building",
                                        City = "Macon",
                                        StateOrProvince = "GA",
                                        PostalCode = "31206"
                                    },
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
