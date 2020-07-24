using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;

namespace Emergence.Service.Interfaces
{
    public interface IOriginService
    {
        Task<Origin> GetOriginAsync(int id);
        Task<IEnumerable<Origin>> GetOriginsAsync();
        Task<Origin> AddOrUpdateOriginAsync(Origin origin);
        Task<Origin> GetOriginAsync(int parentOriginId, string externalId, string altExternalId);
        Task<IEnumerable<Origin>> FindOrigins(string search, string userId, int skip = 0, int take = 10);
    }
}