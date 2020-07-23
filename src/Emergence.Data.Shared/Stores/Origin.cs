using System;

namespace Emergence.Data.Shared.Stores
{
    public class Origin : IIncludable<Origin>, IIncludable<Origin, Origin>, IIncludable<Origin, Location>
    {
        public int Id { get; set; }
        public int? ParentOriginId { get; set; }
        public int? LocationId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public Uri Uri { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string Authors { get; set; }
        public string ExternalId { get; set; }
        public string AltExternalId { get; set; }
        public string UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public Origin ParentOrigin { get; set; }
        public Location Location { get; set; }
    }
}
