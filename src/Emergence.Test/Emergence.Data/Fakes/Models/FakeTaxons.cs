using System.Collections.Generic;
using Emergence.Data.Shared.Models;

namespace Emergence.Test.Data.Fakes.Models
{
    public static class FakeTaxons
    {
        public static IEnumerable<Taxon> Get()
        {
            var taxons = new List<Taxon>
            {
                new Taxon
                {
                    TaxonId = 1,
                    Species = ""
                }
            };
            return taxons;
        }
    }
}
