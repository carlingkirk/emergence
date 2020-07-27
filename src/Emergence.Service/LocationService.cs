using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;
using Emergence.Service.Interfaces;

namespace Emergence.Service
{
    public class LocationService : ILocationService
    {
        private readonly IRepository<Location> _locationRepository;

        public LocationService(IRepository<Location> locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<IEnumerable<Data.Shared.Models.Location>> GetLocationsAsync(IEnumerable<int> ids)
        {
            var locationResult = _locationRepository.GetSomeAsync(l => ids.Any(i => i == l.Id));
            var locations = new List<Data.Shared.Models.Location>();
            await foreach (var location in locationResult)
            {
                locations.Add(location.AsModel());
            }
            return locations;
        }

        public async Task<Data.Shared.Models.Location> AddOrUpdateLocationAsync(Data.Shared.Models.Location location)
        {
            var locationResult = await _locationRepository.AddOrUpdateAsync(l => l.Id == location.LocationId, location.AsStore());
            return locationResult.AsModel();
        }

        public async Task<IEnumerable<Data.Shared.Models.Location>> AddLocationsAsync(IEnumerable<Data.Shared.Models.Location> locations)
        {
            var result = await _locationRepository.AddSomeAsync(locations.Select(l => l.AsStore()));
            return result.Select(l => l.AsModel());
        }
    }
}
