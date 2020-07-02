using System;

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
    }

    public enum OriginType
    {
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
