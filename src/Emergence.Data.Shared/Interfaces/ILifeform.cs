using Emergence.Data.Shared.Models;

namespace Emergence.Data.Shared.Interfaces
{
    public interface ILifeform
    {
        long LifeformId { get; set; }
        Taxon Taxon { get; set; }
        string ScientificName { get; set; }
        string CommonName { get; set; }
    }
}
