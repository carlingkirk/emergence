using System.Collections.Generic;

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
            Name = "Native";
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
    }
}
