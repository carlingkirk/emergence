using Emergence.Data.Stores;

namespace Emergence.Transform.Data
{
    public class Lifeform
    {
        public PlantInfo PlantInfo { get; set; }
        public Taxon Taxon { get; set; }
        public Source Source { get; set; }
        public Origin Origin { get; set; }
    }
}
