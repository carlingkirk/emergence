using System.Collections.Generic;
using Emergence.Data.Shared.Stores;

namespace Emergence.Test.Data.Fakes.Stores
{
    public static class FakePlants
    {
        public static IEnumerable<Lifeform> Plants()
        {
            var plants = new List<Lifeform>
            {
                new Lifeform
                {
                    Id = 0,
                    CommonName = "Dense Blazing Star",
                    ScientificName = "Liatris spicata"
                }
            };

            return plants;
        }

        public static IEnumerable<PlantInfo> PlantInfos()
        {
            var plantInfos = new List<PlantInfo>
            {
                new PlantInfo
                {
                    LifeformId = 0,
                    Id = 0,
                    OriginId = 0,
                    TaxonId = 0,
                    CommonName = "",
                    ScientificName = "",
                    MinimumBloomTime = 7,
                    MaximumBloomTime = 8,
                    MinimumHeight = 1,
                    MaximumHeight = 2,
                    HeightUnit = "Feet",
                    MinimumSpread = 0.75,
                    MaximumSpread = 1.5,
                    SpreadUnit = "Feet",
                    MinimumWater = "Medium",
                    MaximumWater = "Medium",
                    MinimumLight = "PartShade",
                    MaximumLight = "FullSun",
                    MinimumZone = "3",
                    MaximumZone = "8",
                    SeedRefrigerate = false,
                    StratificationStage1 = "",
                    StratificationStage2 = null,
                    StratificationStage3 = null,
                    ScarificationType1 = "Nick",
                    ScarificationType2 = "Soak",
                    ScarificationType3 = null
                }
            };
            return plantInfos;
        }

        public static Taxon Taxon() => new Taxon
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
        };
    }
}
