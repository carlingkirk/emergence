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
                    MinimumLight = "FullShade",
                    MaximumLight = "FullSun",
                    MinimumZone = new Zone { Id = 6, Name = "3" },
                    MaximumZone = new Zone { Id = 21, Name = "8" },
                    StratificationStages = "[{\"Step\":\"1\",\"DayLength\":\"30\",\"StratificationType\":\"ColdMoist\"}]",
                    Visibility = Visibility.Contacts,
                    CreatedBy = Helpers.UserId,
                    ModifiedBy = Helpers.UserId,
                    DateCreated = Helpers.Today.AddDays(Helpers.GetRandom()),
                    DateModified = Helpers.Today.AddHours(1).AddMinutes(5),
                    User = FakeUsers.GetContact(),
                    PlantLocations = FakePlantLocations.Get()
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
                    MinimumHeight = 3,
                    MaximumHeight = 5,
                    HeightUnit = "Feet",
                    MinimumSpread = 2,
                    MaximumSpread = 2.5,
                    SpreadUnit = "Feet",
                    MinimumWater = "Dry",
                    MaximumWater = "Medium",
                    MinimumLight = "PartShade",
                    MaximumLight = "FullSun",
                    MinimumZone = new Zone { Id = 12, Name = "5" },
                    MaximumZone = new Zone { Id = 21, Name = "8" },
                    StratificationStages = "[{\"Step\":\"1\",\"DayLength\":\"1\",\"StratificationType\":\"AbrasionScarify\"},{\"Step\":\"2\",\"DayLength\":\"10\",\"StratificationType\":\"ColdMoist\"}]",
                    Visibility = Visibility.Public,
                    CreatedBy = Helpers.UserId,
                    ModifiedBy = Helpers.UserId,
                    DateCreated = Helpers.Today.AddDays(Helpers.GetRandom()),
                    DateModified = Helpers.Today.AddHours(1).AddMinutes(5),
                    User = FakeUsers.GetPublic()
                },
                new PlantInfo
                {
                    LifeformId = 1,
                    Id = 3,
                    OriginId = 2,
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
                    MinimumLight = "PartSun",
                    MaximumLight = "FullSun",
                    MinimumZone = new Zone { Id = 6, Name = "3" },
                    MaximumZone = new Zone { Id = 21, Name = "8" },
                    StratificationStages = "[{\"Step\":\"1\",\"DayLength\":\"30\",\"StratificationType\":\"ColdMoist\"}]",
                    Visibility = Visibility.Inherit,
                    CreatedBy = FakeUsers.GetPrivate().UserId,
                    ModifiedBy = FakeUsers.GetPrivate().UserId,
                    DateCreated = Helpers.Today.AddDays(Helpers.GetRandom()),
                    DateModified = Helpers.Today.AddHours(1).AddMinutes(5),
                    User = FakeUsers.GetPrivate()
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
