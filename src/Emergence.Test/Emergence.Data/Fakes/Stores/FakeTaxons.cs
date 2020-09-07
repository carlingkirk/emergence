using System.Collections.Generic;
using Emergence.Data.Shared.Stores;

namespace Emergence.Test.Data.Fakes.Stores
{
    public static class FakeTaxons
    {
        public static IEnumerable<Taxon> Get()
        {
            var taxons = new List<Taxon>
            {
                new Taxon
                {
                    Id = 1,
                    Species = ""
                }
            };
            return taxons;
        }
    }
}
