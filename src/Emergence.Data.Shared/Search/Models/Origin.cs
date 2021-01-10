using System;

namespace Emergence.Data.Shared.Search.Models
{
    public class Origin
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Uri { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string Authors { get; set; }
        public string ExternalId { get; set; }
        public string AltExternalId { get; set; }
        public Visibility Visibility { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public Origin ParentOrigin { get; set; }
        public Location Location { get; set; }
        public User User { get; set; }
    }
}
