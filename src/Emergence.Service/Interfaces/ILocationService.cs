using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;

namespace Emergence.Service.Interfaces
{
    public interface ILocationService
    {
        Task<IEnumerable<Location>> GetLocationsAsync(IEnumerable<int> ids);
        Task<Location> AddOrUpdateLocationAsync(Location locations);
        Task<IEnumerable<Data.Shared.Models.Location>> AddLocationsAsync(IEnumerable<Location> locations);
    }
}
