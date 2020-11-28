using System.Collections.Generic;

namespace Emergence.Data.Shared.Search.Models
{
    public class Lifeform
    {
        public IEnumerable<TaxonCategory> TaxonCategories { get; set; }
        public IEnumerable<Synonym> Synonyms { get; set; }
        public IEnumerable<PlantLocation> PlantLocations { get; set; }
        public IEnumerable<Zone> Zones { get; set; }
        public IEnumerable<Sublifeform> Lifeforms { get; set; }
        public IEnumerable<PlantInfo> PlantInfos { get; set; }
    }
}
