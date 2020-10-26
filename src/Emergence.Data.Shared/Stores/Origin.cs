using System;
using System.ComponentModel.DataAnnotations;

namespace Emergence.Data.Shared.Stores
{
    public class Origin : IIncludable<Origin>, IIncludable<Origin, Origin>, IIncludable<Origin, Location>, IAuditable, IVisibile<Origin>
    {
        public int Id { get; set; }
        public int? ParentOriginId { get; set; }
        public int? LocationId { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        [StringLength(36)]
        public string Type { get; set; }
        public string Description { get; set; }
        public Uri Uri { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        [StringLength(200)]
        public string Authors { get; set; }
        [StringLength(100)]
        public string ExternalId { get; set; }
        [StringLength(100)]
        public string AltExternalId { get; set; }
        public Visibility Visibility { get; set; }
        [StringLength(36)]
        public string CreatedBy { get; set; }
        [StringLength(36)]
        public string ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public Origin ParentOrigin { get; set; }
        public Location Location { get; set; }
        public User User { get; set; }
    }
}
