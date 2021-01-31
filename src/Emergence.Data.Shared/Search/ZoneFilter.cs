using System.Collections.Generic;
using System.Linq;
using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Search
{
    public class ZoneFilter : Filter, IFilterDisplay<string>
    {
        public IDictionary<string, long?> FacetValues { get; set; }
        public int Value { get; set; }
        public ZoneFilter()
        {
            Name = "Zone";
            InputType = InputType.Select;
            FilterType = FilterType.Integer;
            FacetValues = ZoneHelper.GetZones().Select(z => z.Id).ToDictionary(m => m.ToString(), c => (long?)0L);
        }

        public string DisplayValue(string value, long? count = null)
        {
            if (int.TryParse(value, out var zoneId))
            {
                var zone = ZoneHelper.GetZones().First(z => z.Id == zoneId).Name;
                return count != null ? $"{zone} ({count})" : $"{zone}";
            }
            else
            {
                return value.ToString();
            }
        }
    }
}
