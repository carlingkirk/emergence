using Emergence.Data.Shared.Models;

namespace Emergence.Data.Shared.Interfaces
{
    public interface ILifeform
    {
        int LifeformId { get; set; }
        Taxon Taxon { get; set; }
        string ScientificName { get; set; }
        string CommonName { get; set; }
    }
}
