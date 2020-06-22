using Emergence.Data.Models;

namespace Emergence.Data.Interfaces
{
    public interface ILifeform
    {
        long LifeformId { get; set; }
        Taxon Taxon { get; set; }
        string ScientificName { get; set; }
        string CommonName { get; set; }
    }
}
