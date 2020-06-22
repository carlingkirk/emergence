using Emergence.Data.Models;
using System.Collections.Generic;

namespace Emergence.Test.Fakes
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
                    Taxon = new Taxon
                    {
                        Kingdom = "Plantae",
                        Phylum = "Tracheophyta",
                        Subphylum = "Angiospermae",
                        Class = "Magnoliopsida",
                        Subclass = null,
                        Order = "Asterales",
                        Superfamily = null,
                        Family = "Asteraceae",
                        Subfamily = "Asteroideae",
                        Tribe = "Eupatorieae",
                        Subtribe = "Liatrinae",
                        Genus = "Liatris",
                        Subgenus = null,
                        Species = "spicata",
                        Subspecies = null,
                        Variety = "spicata",
                        Form = null
                    },
                    BloomTime = "Jul-Aug",
                    Height = "",
                    Spread = "",
                    Requirements = new Requirements
                    {
                        LightRequirements = new LightRequirements
                        {
                            MinimumLight = LightTypes.PartShade,
                            MaximumLight = LightTypes.FullSun
                        },
                        WaterRequirements = new WaterRequirements
                        {
                            MinimumWater = WaterTypes.Medium,
                            MaximumWater = WaterTypes.Medium
                        },
                        SoilRequirements = null,
                        ScarificationRequirements = "Nick",
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
                        SeedStorageRequirements = null
                    },
                    Zones = "3-8"
                }
            };

            return plants;
        }
    }
}
