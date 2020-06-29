using System.Collections.Generic;
using System.Linq;

namespace Emergence.Data.External.iNaturalist
{
    public class Geolocation
    {
        public Geolocation(IEnumerable<float> coordinates, string place = "")
        {
            if (coordinates != null && coordinates.Any() && coordinates.Count() == 2)
            {
                Latitude = coordinates.First();
                Longitude = coordinates.Skip(1).First();
            }
            Place = place;
        }

        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string Place { get; set; }
    }
}
