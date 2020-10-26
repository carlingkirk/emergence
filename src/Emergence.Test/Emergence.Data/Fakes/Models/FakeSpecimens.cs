using System;
using System.Collections.Generic;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Test.Data.Fakes.Models
{
    public static class FakeSpecimens
    {
        public static IEnumerable<Specimen> Get()
        {
            var specimens = new List<Specimen>
            {
                new Specimen
                {
                    SpecimenId = 1,
                    Lifeform = new Lifeform
                    {
                        LifeformId = 1,
                        CommonName = "Dense Blazing Star",
                        ScientificName = "Liatris spicata",
                    },
                    InventoryItem = new InventoryItem
                    {
                        Inventory = new Inventory { InventoryId = 1, OwnerId = Helpers.UserId },
                        InventoryItemId = 1,
                        DateAcquired = new DateTime(2020,06,26),
                        ItemType = ItemType.Specimen,
                        Name = "Liatris spicata Seeds",
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
                        PlantInfoId = 1,
                        LifeformId = 1,
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
                            StratificationStages = new List<StratificationStage>
                            {
                                new StratificationStage
                                {
                                    Step = 1,
                                    DayLength = 0,
                                    StratificationType = StratificationType.NickScarify
                                },
                                new StratificationStage
                                {
                                    Step = 2,
                                    DayLength = 30,
                                    StratificationType = StratificationType.ColdMoist
                                }
                            },
                            ZoneRequirements = new ZoneRequirements
                            {
                                MinimumZone = new Zone { Number = 3 },
                                MaximumZone = new Zone { Number = 8 }
                            }
                        },
                        Origin = new Origin
                        {
                            OriginId = 1,
                        },
                        Taxon = new Taxon
                        {
                            TaxonId = 1
                        },
                        DateCreated = DateTime.UtcNow,
                        DateModified = null
                    },
                    SpecimenStage = SpecimenStage.Seed
                }
            };

            return specimens;
        }
    }
}
