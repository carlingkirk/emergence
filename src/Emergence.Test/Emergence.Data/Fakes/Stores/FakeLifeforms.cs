using System.Collections.Generic;
using Emergence.Data.Shared.Stores;

namespace Emergence.Test.Data.Fakes.Stores
{
    public static class FakeLifeforms
    {
        public static IEnumerable<Lifeform> Get()
        {
            var lifeforms = new List<Lifeform>
            {
                new Lifeform
                {
                    Id = 1,
                    CommonName = "Dense Blazing Star",
                    ScientificName = "Liatris spicata"
                },
                new Lifeform
                {
                    Id = 2,
                    CommonName = "Butterfly Weed",
                    ScientificName = "Asclepias tuberosa"
                },
                new Lifeform
                {
                    Id = 3,
                    CommonName = "Crossvine",
                    ScientificName = "Bignonia capreolata"
                },
                new Lifeform
                {
                    Id = 4,
                    CommonName = "common morning-glory",
                    ScientificName = "Ipomoea purpurea"
                }
            };

            return lifeforms;
        }
    }
}
