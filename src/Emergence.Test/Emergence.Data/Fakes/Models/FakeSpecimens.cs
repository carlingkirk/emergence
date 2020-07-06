using System;
using System.Collections.Generic;
using Emergence.Data.Shared.Models;

namespace Emergence.Test.Data.Fakes.Models
{
    public static class FakeSpecimens
    {
        public static IEnumerable<Specimen> Specimens()
        {
            var specimens = new List<Specimen>
            {
                new Specimen
                {
                    InventoryItem = new InventoryItem
                    {
                        Inventory = new Inventory { InventoryId = 0 },
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
                    PlantInfo = new PlantInfo
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
                            Unit = DistanceUnit.Feet
                        },
                        Spread = new Spread
                        {
                            MinimumSpread = .75,
                            MaximumSpread = 1.5,
                            Unit = DistanceUnit.Feet
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
                                        new StratificationStage { DayLength = 60, MinimumTemperature = 37, MaximumTemperature = 43, TemperatureUnit = TemperatureUnit.Fahrenheit }
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
