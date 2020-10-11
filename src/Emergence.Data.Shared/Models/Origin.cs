using System;

namespace Emergence.Data.Shared.Models
{
    public class Origin
    {
        public int OriginId { get; set; }

        public string Name { get; set; }
        public OriginType Type { get; set; }
        public string Description { get; set; }
        public string Authors { get; set; }
        public Uri Uri { get; set; }
        public int? LocationId { get; set; }
        public string ExternalId { get; set; }
        public string AltExternalId { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public string ShortUri => Uri != null ? Uri.ToString().Length > 40 ? Uri.ToString().Substring(0, 40) + "..." : Uri.ToString() : null;

        public Location Location { get; set; }
        public Origin ParentOrigin { get; set; }
    }
}
