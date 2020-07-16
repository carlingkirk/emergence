using System;
using System.ComponentModel;

namespace Emergence.Data.Shared.Models
{
    public class Origin
    {
        public int OriginId { get; set; }
        public Origin ParentOrigin { get; set; }
        public string Name { get; set; }
        public OriginType Type { get; set; }
        public string Description { get; set; }
        public string Authors { get; set; }
        public Uri Uri { get; set; }
        public Location Location { get; set; }
        public string ExternalId { get; set; }
        public string AltExternalId { get; set; }
        public string UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }

    public enum OriginType
    {
        [Description("")]
        Unknown,
        Nursery,
        Store,
        Location,
        Person,
        Event,
        Website,
        Webpage,
        Publication
    }
}
