using System;
using System.Linq;
using System.Linq.Expressions;
using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Search
{
    [TypeDiscriminator("Zone")]
    public class ZoneFilter : SelectFilter<int>, IFilterDisplay<int>
    {
        public ZoneFilter(Filter<int> filter)
        {
            Name = filter.Name;
            InputType = filter.InputType;
            FilterType = filter.FilterType;
            Value = filter.Value;
        }

        public ZoneFilter()
        {
            Name = "Zone";
            InputType = InputType.Select;
            FilterType = FilterType.Integer;
            Values = ZoneHelper.GetZones().Select(z => z.Id);
        }

        public Expression<Func<PlantInfo, bool>> Filter => p =>
            p.MinimumZone != null && p.MinimumZone.Id <= Value &&
            p.MaximumZone != null && p.MaximumZone.Id >= Value;

        public string DisplayValue(int value, long? count = null) => ZoneHelper.GetZones().First(z => z.Id == value).Name;
    }
}
