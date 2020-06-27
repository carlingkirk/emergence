using Emergence.Data.Shared.Models;
using System.Collections.Generic;
using System.Net.Mail;

namespace Emergence.Test.Data.Fakes.Models.Plants
{
    public static class Get
    {
        public static IEnumerable<Plant> Plants()
        {
            var plants = new List<Plant>
            {
                new Plant
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
                    },
                    Taxon = new Taxon
                    {

                    }
                }
            };

            return plants;
        }
    }
}
