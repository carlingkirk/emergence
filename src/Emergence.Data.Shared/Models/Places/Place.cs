using System.Collections.Generic;
using Emergence.Data.Shared.Models.Places;

namespace Emergence.Data.Shared.Models
{
    public class Place
    {
        public virtual string PlaceId { get; set; }
        public virtual Geometry Geometry { get; set; }
        public virtual string FormattedAddress { get; set; }
        public virtual bool PartialMatch { get; set; }
        public PlusCode PlusCode { get; set; }
        public virtual IEnumerable<string> PostcodeLocalities { get; set; }
        public virtual IEnumerable<PlaceLocationType> Types { get; set; }
        public virtual IEnumerable<AddressComponent> AddressComponents { get; set; }
    }
}
