using System.Collections.Generic;
using Emergence.Data.Shared.Models;

namespace Emergence.Test.Data.Fakes.Models
{
    public static class FakePlants
    {
        public static IEnumerable<PlantInfo> Plants()
        {
            var plantInfos = new List<PlantInfo>
            {
                new PlantInfo
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
                    },
                    Taxon = new Taxon
                    {

                    }
                }
            };

            return plantInfos;
        }
    }
}
