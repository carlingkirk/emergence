using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;

namespace Emergence.API.Services.Interfaces
{
    public interface ISpecimenService
    {
        Task<Specimen> GetSpecimenAsync(long specimenId);
        Task<IEnumerable<Specimen>> GetSpecimensForInventoryAsync(int inventoryId);
        Task<Specimen> AddOrUpdateAsync(Specimen specimen, string userId);
        Task<IEnumerable<Specimen>> GetSpecimensByIdsAsync(IEnumerable<int> specimenIds);
    }
}
