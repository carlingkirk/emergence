using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;

namespace Emergence.API.Services.Interfaces
{
    public interface ISpecimenService
    {
        Task<Specimen> GetSpecimenAsync(long specimenId);
        Task<IEnumerable<Specimen>> GetSpecimensAsync(int inventoryId);
        Task<Specimen> AddOrUpdateAsync(Specimen specimen);
    }
}
