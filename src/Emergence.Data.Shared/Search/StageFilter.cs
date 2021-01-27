using System;
using System.Collections.Generic;
using System.Linq;
using Emergence.Data.Shared.Extensions;

namespace Emergence.Data.Shared.Search
{
    [TypeDiscriminator("Stage")]
    public class StageFilter : SelectFilter<string>
    {
        public StageFilter(Filter<string> filter)
        {
            Name = filter.Name;
            InputType = filter.InputType;
            FilterType = filter.FilterType;
            Value = filter.Value;
        }

        public StageFilter()
        {
            Name = "Stage";
            InputType = InputType.Select;
            FilterType = FilterType.String;
            var values = new List<string> { "" };
            values.AddRange(Enum.GetValues(typeof(SpecimenStage)).Cast<SpecimenStage>().Select(s => s.ToString()));
            FacetValues = values.ToDictionary(m => m, c => (long?)0L);
        }

        public string DisplayValue(string value, long? count = null)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            else
            {
                var stageValue = Enum.Parse<SpecimenStage>(value);
                return count != null ? $"{stageValue.ToFriendlyName()} ({count})" : $"{stageValue.ToFriendlyName()}";
            }
        }
    }
}
