using System.Collections.Generic;
using Emergence.Data.Shared.Models;

namespace Emergence.Test.Data.Fakes.Models
{
    public static class FakePlantInfos
    {
        public static IEnumerable<PlantInfo> Get()
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
                        SoilRequirements = new List<SoilType>
                        {
                            SoilType.Fertile
                        },
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
                    Taxon = new Taxon
                    {

                    }
                }
            };

            return plantInfos;
        }
    }
}
