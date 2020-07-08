using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;

namespace Emergence.API.Services.Interfaces
{
    public interface IOriginService
    {
        Task<Origin> GetOriginAsync(int id);
        Task<IEnumerable<Origin>> GetOriginsAsync();
        Task<Origin> AddOrUpdateOriginAsync(Origin origin);
    }
}
