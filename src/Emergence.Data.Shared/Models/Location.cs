using System;

namespace Emergence.Data.Shared.Models
{
    public class Location
    {
        public int LocationId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string StateOrProvince { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public double? Altitude { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public bool HasAddressInfo => !string.IsNullOrEmpty(AddressLine1) || !string.IsNullOrEmpty(CityState) || !string.IsNullOrEmpty(PostalCode) || !string.IsNullOrEmpty(Country);
        public string LatLong => Latitude.HasValue && Longitude.HasValue ? Latitude.ToString() + ", " + Longitude.ToString() : "";
        public string CityState => (City != null) ? (City + (StateOrProvince != null ? ", " + StateOrProvince : "")) : "" + StateOrProvince;
    }
}
