using System;
using System.Collections.Generic;
using System.Text;

namespace Emergence.Data.Stores
{
    public class Origin
    {
        public long OriginId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Uri Uri { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
    }

    public class Source
    {
        public long OriginId { get; set; }
        public long SourceId { get; set; }
        public string Name { get; set; }
        public Uri Uri { get; set; }
        public string ExternalId { get; set; }
        public string AltExternalId { get; set; }
    }
}
