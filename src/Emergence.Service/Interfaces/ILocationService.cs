using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;

namespace Emergence.Service.Interfaces
{
    public interface ILocationService
    {
        Task<IEnumerable<Location>> GetLocationsAsync(IEnumerable<int> ids);
        Task<IEnumerable<Location>> GetLocationsAsync(Expression<Func<Data.Shared.Stores.Location, bool>> predicate);
        Task<Location> AddOrUpdateLocationAsync(Location locations);
        Task<IEnumerable<Location>> AddLocationsAsync(IEnumerable<Location> locations);
    }
}
