using Emergence.Data.Shared.Models;
using System;
using System.Collections.Generic;

namespace Emergence.Test.Data.Fakes.Models.Specimens
{
    public static class Get
    {
        public static IEnumerable<Specimen> Specimens()
        {
            var specimens = new List<Specimen>
            {
                new Specimen
                {
                    InventoryItem = new InventoryItem
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
                    },
                    Lifeform = new Plant
                    {
                        LifeformId = 0,
                        CommonName = "Dense Blazing Star",
                        ScientificName = "Liatris spicata",
                        BloomTime = new BloomTime
                        {
                            MinimumBloomTime = Month.Jul,
                            MaximumBloomTime = Month.Aug
                        },
                        Height = new Height
                        {
                            MinimumHeight = 1,
                            MaximumHeight = 2,
                            Unit = Unit.Feet
                        },
                        Spread = new Spread
                        {
                            MinimumSpread = .75,
                            MaximumSpread = 1.5,
                            Unit = Unit.Feet
                        },
                        Requirements = new Requirements
                        {
                            LightRequirements = new LightRequirements
                            {
                                MinimumLight = LightType.PartShade,
                                MaximumLight = LightType.FullSun
                            },
                            WaterRequirements = new WaterRequirements
                            {
                                MinimumWater = WaterType.Medium,
                                MaximumWater = WaterType.Medium
                            },
                            SoilRequirements = null,
                            ScarificationRequirements = new ScarificationRequirements
                            {
                                ScarificationTypes = new List<ScarificationType>() { ScarificationType.Nick }
                            },
                            StratificationRequirements = new StratificationRequirements
                            {
                                StratificationStages = new Dictionary<int, StratificationStage>()
                                {
                                    {
                                        0,
                                        new StratificationStage { DayLength = 60, MinimumTemperature = 37, MaximumTemperature = 43 }
                                    }
                                }
                            },
                            SeedStorageRequirements = new SeedStorageRequirements { Refrigerate = false },
                            ZoneRequirements = new ZoneRequirements
                            {
                                MinimumZone = new Zone { Number = 3 },
                                MaximumZone = new Zone { Number = 8 }
                            }
                        }
                    },
                    SpecimenStage = SpecimenStage.Seed
                }
            };

            return specimens;
        }
    }
}
