using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Service.Interfaces
{
    public interface IOriginService
    {
        Task<Origin> GetOriginAsync(int id, User user);
        Task<Origin> GetOriginAsync(int id);
        Task<IEnumerable<Origin>> GetOriginsOfParentAsync(int parentOriginId);
        Task<Origin> AddOrUpdateOriginAsync(Origin origin, string userId);
        Task<Origin> GetOriginAsync(int parentOriginId, string externalId, string altExternalId);
        Task<FindResult<Origin>> FindOrigins(FindParams findParams, User user);
        Task<IEnumerable<Origin>> AddOriginsAsync(IEnumerable<Origin> origins);
        Task RemoveOriginAsync(Origin origin);
    }
}
