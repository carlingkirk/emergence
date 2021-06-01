using System.Collections.Generic;
using System.Linq;
using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Search
{
    public class ZoneFilter : SelectFilter<string>, IFilterDisplay<string>
    {
        private readonly IEnumerable<Zone> Zones = ZoneHelper.GetZones();

        public ZoneFilter()
        {
            Name = "Zone";
            InputType = InputType.Select;
            FilterType = FilterType.String;
            FacetValues = Zones.Select(z => z.Id).ToDictionary(m => m.ToString(), c => (long?)0L);
        }

        public override Dictionary<string, long?> GetFacetValues(Dictionary<string, long?> values)
        {
            var facetValues = values.ToDictionary(k => Zones.FirstOrDefault(z => z.Id.ToString() == k.Key)?.Name ?? "", v => v.Value).OrderBy(k => k.Key).ToDictionary(k => k.Key, v => v.Value);

            if (!facetValues.Any(v => v.Key == ""))
            {
                facetValues = facetValues.Prepend(new KeyValuePair<string, long?>("", null)).ToDictionary(k => k.Key, v => v.Value);
            }

            return facetValues;
        }

        public string GetFacetValue(string value)
        {
            var zone = Zones.FirstOrDefault(v => v.Name == value);
            return zone?.Id.ToString() ?? "0";
        }

        public string DisplayValue(string value, long? count = null)
        {
            if (int.TryParse(value, out var zoneId))
            {
                if (zoneId > 0)
                {
                    var zone = ZoneHelper.GetZones().First(z => z.Id == zoneId).Name;
                    return count != null ? $"{zone} ({count})" : $"{zone}";
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return value.ToString();
            }
        }
    }
}
