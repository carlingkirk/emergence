using System.Collections.Generic;
using Emergence.Data.Shared.Models;

namespace Emergence.Test.Emergence.Data.Fakes.Models
{
    public static class FakeLifeforms
    {
        public static IEnumerable<Lifeform> Lifeforms()
        {
            var lifeforms = new List<Lifeform>
            {
                new Lifeform
                {
                    LifeformId = 1,
                    CommonName = "",
                    Origin = new Origin
                    {

                    },
                    PlantInfo = new PlantInfo
                    {

                    },
                    ScientificName = "",
                    Taxon = new Taxon
                    {

                    }
                }
            };
            return lifeforms;
        }
    }
}
