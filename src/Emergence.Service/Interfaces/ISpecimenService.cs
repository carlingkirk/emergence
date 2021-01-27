using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Service.Interfaces
{
    public interface ISpecimenService
    {
        Task<Specimen> GetSpecimenAsync(int specimenId, User user);
        Task<Specimen> AddOrUpdateAsync(Specimen specimen, string userId);
        Task<SpecimenFindResult> FindSpecimens(SpecimenFindParams findParams, User user);
        Task RemoveSpecimenAsync(Specimen specimen);
    }
}
