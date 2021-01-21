using System.Collections.Generic;
using System.Linq;
using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Search
{
    public class ZoneFilter : Filter
    {
        public IDictionary<int, long?> FacetValues { get; set; }
        public int Value { get; set; }
        public ZoneFilter()
        {
            Name = "Zone";
            InputType = InputType.Select;
            FilterType = FilterType.Integer;
            FacetValues = ZoneHelper.GetZones().Select(z => z.Id).ToDictionary(m => m, c => (long?)0L);
        }

        public string DisplayValue(int value, long? count = null) => ZoneHelper.GetZones().First(z => z.Id == value).Name;
    }
}
