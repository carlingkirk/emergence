using System.Collections.Generic;

namespace Emergence.Data.Shared.Models.Places
{
    public class AddressComponent
    {
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public IEnumerable<AddressComponentType> Types { get; set; }
    }
}
