using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Service.Interfaces
{
    public interface ISpecimenService
    {
        Task<Specimen> GetSpecimenAsync(int specimenId, User user);
        Task<IEnumerable<Specimen>> GetSpecimensForInventoryAsync(int inventoryId, User user);
        Task<Specimen> AddOrUpdateAsync(Specimen specimen, string userId);
        Task<IEnumerable<Specimen>> GetSpecimensByIdsAsync(IEnumerable<int> specimenIds);
        Task<FindResult<Specimen>> FindSpecimens(FindParams findParams, User user);
        Task RemoveSpecimenAsync(Specimen specimen);
    }
}
