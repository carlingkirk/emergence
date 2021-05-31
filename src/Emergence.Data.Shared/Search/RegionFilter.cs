using System.Collections.Generic;

namespace Emergence.Data.Shared.Search
{
    [TypeDiscriminator("Region")]
    public class RegionFilter : SelectFilter<string>, IFilterDisplay<string>
    {
        public RegionFilter(Filter<string> filter)
        {
            Name = filter.Name;
            InputType = filter.InputType;
            FilterType = filter.FilterType;
            Value = filter.Value;
        }

        public RegionFilter()
        {
            Name = "Region";
            InputType = InputType.Select;
            FilterType = FilterType.String;
            FacetValues = new Dictionary<string, long?>();
        }

        public override Dictionary<string, long?> GetFacetValues(Dictionary<string, long?> values) => values;

        public string DisplayValue(string value, long? count = null)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            else if (value == "0")
            {
                return string.Empty;
            }
            else
            {
                return count != null ? $"{value} ({count})" : $"{value}";
            }
        }
    }
}
