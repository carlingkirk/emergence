namespace Emergence.Data.Shared.Models
{
    public class Lifeform
    {
        public int LifeformId { get; set; }
        public InventoryItem InventoryItem { get; set; }
        public Taxon Taxon { get; set; }
        public string ScientificName { get; set; }
        public string CommonName { get; set; }
        public PlantInfo PlantInfo { get; set; }
        public Origin Origin { get; set; }
    }
}
