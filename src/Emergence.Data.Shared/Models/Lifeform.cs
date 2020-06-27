namespace Emergence.Data.Shared.Models
{
    public class Lifeform
    {
        public long LifeformId { get; set; }
        public Taxon Taxon { get; set; }
        public string ScientificName { get; set; }
        public string CommonName { get; set; }
    }
}
