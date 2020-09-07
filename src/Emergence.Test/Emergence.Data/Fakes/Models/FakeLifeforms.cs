using System.Collections.Generic;
using Emergence.Data.Shared.Models;

namespace Emergence.Test.Data.Fakes.Models
{
    public static class FakeLifeforms
    {
        public static IEnumerable<Lifeform> Get()
        {
            var lifeforms = new List<Lifeform>
            {
                new Lifeform
                {
                    LifeformId = 1,
                    CommonName = "",
                    ScientificName = ""
                }
            };
            return lifeforms;
        }
    }
}
