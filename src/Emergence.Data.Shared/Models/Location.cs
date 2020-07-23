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

        public string CityState => City + ", " + StateOrProvince;
    }
}
