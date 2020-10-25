using System.Collections.Generic;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Stores;

namespace Emergence.Test.Data.Fakes.Stores
{
    public static class FakePlantInfos
    {
        public static IEnumerable<PlantInfo> Get()
        {
            var plantInfos = new List<PlantInfo>
            {
                new PlantInfo
                {
                    LifeformId = 1,
                    Id = 1,
                    OriginId = 1,
                    TaxonId = Taxon().Id,
                    CommonName = "Dense Blazing Star",
                    ScientificName = "Liatris spicata",
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
                    StratificationStages = "[{\"Step\":\"1\",\"DayLength\":\"30\",\"StratificationType\":\"ColdMoist\"}]",
                    Visibility = Visibility.Contacts,
                    CreatedBy = "me",
                    ModifiedBy = "me",
                    DateCreated = Helpers.Today.AddDays(Helpers.GetRandom()),
                    DateModified = Helpers.Today.AddHours(1).AddMinutes(5)
                },
                new PlantInfo
                {
                    LifeformId = 2,
                    Id = 2,
                    OriginId = 2,
                    TaxonId = Taxon().Id,
                    CommonName = "White wild indigo",
                    ScientificName = "Baptisia alba",
                    MinimumBloomTime = 4,
                    MaximumBloomTime = 5,
                    MinimumHeight = 2,
                    MaximumHeight = 4,
                    HeightUnit = "Feet",
                    MinimumSpread = 2,
                    MaximumSpread = 2.5,
                    SpreadUnit = "Feet",
                    MinimumWater = "Dry",
                    MaximumWater = "Medium",
                    MinimumLight = "PartShade",
                    MaximumLight = "FullSun",
                    MinimumZone = "5",
                    MaximumZone = "8",
                    StratificationStages = "[{\"Step\":\"1\",\"DayLength\":\"1\",\"StratificationType\":\"AbrasionScarify\"},{\"Step\":\"2\",\"DayLength\":\"10\",\"StratificationType\":\"ColdMoist\"}]",
                    Visibility = Visibility.Public,
                    CreatedBy = "me",
                    ModifiedBy = "me",
                    DateCreated = Helpers.Today.AddDays(Helpers.GetRandom()),
                    DateModified = Helpers.Today.AddHours(1).AddMinutes(5)
                }
            };
            return plantInfos;
        }

        public static Taxon Taxon() => new Taxon
        {
            Id = 1,
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
