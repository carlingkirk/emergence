using System.Collections.Generic;
using Emergence.Data.Shared.Models;

namespace Emergence.Test.Data.Fakes.Models
{
    public static class FakeLocations
    {
        public static IEnumerable<Location> Get()
        {
            var locations = new List<Location>
            {
                new Location
                {
                    LocationId = 1,
                    AddressLine1 = "100 University Parkway",
                    AddressLine2 = "Charles H. Jones Building",
                    City = "Macon",
                    StateOrProvince = "GA",
                    PostalCode = "31206",
                    Country = "United States",
                    Latitude = 32.8087681,
                    Longitude = -83.7358335
                }
            };
            return locations;
        }
    }
}
