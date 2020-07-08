using System;

namespace Emergence.Data.Shared.Stores
{
    public class Origin
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public Uri Uri { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string Authors { get; set; }
        public string ExternalId { get; set; }
        public string AltExternalId { get; set; }
    }
}
