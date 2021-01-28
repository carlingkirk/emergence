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
        }

        public string DisplayValue(string value, long? count = null)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            else
            {
                return count != null ? $"{value} ({count})" : $"{value}";
            }
        }
    }
}
