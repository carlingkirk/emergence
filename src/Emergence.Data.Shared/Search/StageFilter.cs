using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json.Serialization;
using Emergence.Data.Shared.Stores;

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

        [JsonIgnore]
        public Expression<Func<Specimen, bool>> Filter => s => s.SpecimenStage != null && s.SpecimenStage == Value;
    }
}
