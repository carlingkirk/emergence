namespace Emergence.Data.Shared.Search.Models
{
    public class PlantLocation
    {
        public LocationStatus Status { get; set; }
        public string City { get; set; }
        public string StateOrProvince { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
    }
}
