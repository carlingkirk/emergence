using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Emergence.Data
{
    public class Source
    {
        public long SourceId { get; set; }
        public byte SourceTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Uri Uri { get; set; }
        public Location Location { get; set; }
    }

    public class SourceType
    {
        public byte SourceTypeId { get; set; }
        public string Name { get; set; }
    }
}
