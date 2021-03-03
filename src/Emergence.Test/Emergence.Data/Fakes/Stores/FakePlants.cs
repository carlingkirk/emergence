using System.Collections.Generic;
using System.Linq;
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
                    Origin = FakeOrigins.Get().First(),
                    TaxonId = Taxon().Id,
                    Taxon = Taxon(),
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
                    MinimumZone = new Zone { Id = 7, Name = "3" },
                    MaximumZone = new Zone { Id = 22, Name = "8" },
                    StratificationStages = "[{\"Step\":\"1\",\"DayLength\":\"30\",\"StratificationType\":\"ColdMoist\"}]",
                    SoilTypes = "[\"Loamy\",\"Rocky\",\"Clay\"]",
                    WildlifeEffects = "[{\"Wildlife\":\"Bees\",\"Effect\":\"Food\"}]",
                    Notes = "Liatris spicata, the dense blazing star or prairie gay feather, is an herbaceous perennial flowering plant in the sunflower and daisy family " +
                    "Asteraceae. It is native to eastern North America where it grows in moist prairies and sedge meadows. The plants have tall spikes of purple flowers " +
                    "resembling bottle brushes or feathers that grow one to five feet tall. The species grows in hardiness zones 3 - 8, stretching from the Midwest " +
                    "to the East Coast, eastern and western Canada.",
                    Visibility = Visibility.Contacts,
                    CreatedBy = Helpers.UserId,
                    ModifiedBy = Helpers.UserId,
                    DateCreated = Helpers.Today.AddDays(Helpers.GetRandom()),
                    DateModified = Helpers.Today.AddHours(1).AddMinutes(5),
                    User = FakeUsers.GetContact(),
                    PlantLocations = FakePlantLocations.Get(),
                    Lifeform = FakeLifeforms.Get().First()
                },
                new PlantInfo
                {
                    LifeformId = 2,
                    Id = 2,
                    OriginId = 2,
                    Origin = FakeOrigins.Get().Skip(1).First(),
                    TaxonId = Taxon().Id,
                    Taxon = Taxon(),
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
                    MinimumZone = new Zone { Id = 13, Name = "5" },
                    MaximumZone = new Zone { Id = 22, Name = "8" },
                    StratificationStages = "[{\"Step\":\"1\",\"DayLength\":\"1\",\"StratificationType\":\"AbrasionScarify\"},{\"Step\":\"2\",\"DayLength\":\"10\",\"StratificationType\":\"ColdMoist\"}]",
                    WildlifeEffects = "[{\"Wildlife\":\"Butterflies\",\"Effect\":\"Host\"},{\"Wildlife\":\"Bees\",\"Effect\":\"Food\"}]",
                    Notes = "Baptisia alba, commonly called white wild indigo or white false indigo, is a herbaceous plant in the bean family Fabaceae. It is native from central and " +
                            "eastern North America. There are two varieties, Baptisia alba var. alba and Baptisia alba var. macrophylla.",
                    Visibility = Visibility.Public,
                    CreatedBy = Helpers.UserId,
                    ModifiedBy = Helpers.UserId,
                    DateCreated = Helpers.Today.AddDays(Helpers.GetRandom()),
                    DateModified = Helpers.Today.AddHours(1).AddMinutes(5),
                    User = FakeUsers.GetPublic(),
                    Lifeform = FakeLifeforms.Get().Skip(1).First()
                },
                new PlantInfo
                {
                    LifeformId = 1,
                    Id = 3,
                    OriginId = 2,
                    Origin = FakeOrigins.Get().Skip(2).First(),
                    TaxonId = Taxon().Id,
                    Taxon = Taxon(),
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
                    MinimumZone = new Zone { Id = 7, Name = "3" },
                    MaximumZone = new Zone { Id = 22, Name = "8" },
                    StratificationStages = "[{\"Step\":\"1\",\"DayLength\":\"30\",\"StratificationType\":\"ColdMoist\"}]",
                    WildlifeEffects = "[{\"Wildlife\":\"Birds\",\"Effect\":\"Food\"}]",
                    Notes = "Liatris spicata, the dense blazing star or prairie gay feather, is an herbaceous perennial flowering plant in the sunflower and daisy family " +
                    "Asteraceae. It is native to eastern North America where it grows in moist prairies and sedge meadows. The plants have tall spikes of purple flowers " +
                    "resembling bottle brushes or feathers that grow one to five feet tall. The species grows in hardiness zones 3 - 8, stretching from the Midwest " +
                    "to the East Coast, eastern and western Canada.",
                    Visibility = Visibility.Inherit,
                    CreatedBy = FakeUsers.GetPrivate().UserId,
                    ModifiedBy = FakeUsers.GetPrivate().UserId,
                    DateCreated = Helpers.Today.AddDays(Helpers.GetRandom()),
                    DateModified = Helpers.Today.AddHours(1).AddMinutes(5),
                    User = FakeUsers.GetPrivate(),
                    Lifeform = FakeLifeforms.Get().First()
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
