using System.Collections.Generic;
using Emergence.Data.Shared.Stores;

namespace Emergence.Test.Data.Fakes.Stores
{
    public static class FakeSynonyms
    {
        public static IEnumerable<Synonym> Get()
        {
            var synonyms = new List<Synonym>
            {
                new Synonym
                {
                    Id = 1,
                    Name = "Plants",
                    DateUpdated = Helpers.Today,
                    Language = "English",
                    Origin = new Origin
                    {
                        Id  = 89983,
                        Type = "Database"
                    },
                    OriginId = 89983,
                    Rank = "",
                    Taxon = new Taxon
                    {
                        Kingdom = "Plantae"
                    },
                    TaxonId = 1
                }
            };
            return synonyms;
        }
    }
}
