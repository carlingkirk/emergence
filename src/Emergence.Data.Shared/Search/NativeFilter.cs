using System.Collections.Generic;
using System.Linq;

namespace Emergence.Data.Shared.Search
{
    public class NativeFilter : SelectFilter<string>, IFilterDisplay<string>
    {
        public LocationStatus Status { get; set; }

        public NativeFilter(Filter<string> filter)
        {
            Name = filter.Name;
            InputType = filter.InputType;
            FilterType = filter.FilterType;
            Value = filter.Value;
        }

        public NativeFilter()
        {
            Name = "Native to";
            Status = LocationStatus.Native;
            InputType = InputType.Select;
            FilterType = FilterType.String;
            FacetValues = new Dictionary<string, long?>();
        }

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

        public override Dictionary<string, long?> GetFacetValues(Dictionary<string, long?> values)
        {
            if (!values.Any(v => v.Key == ""))
            {
                values = values.Prepend(new KeyValuePair<string, long?>("", null)).ToDictionary(k => k.Key, v => v.Value);
            }
            return values;
        }
    }
}
