using System;
using System.Collections.Generic;
using System.Linq;
using Emergence.Data.Shared.Extensions;

namespace Emergence.Data.Shared.Search
{
    [TypeDiscriminator("Light")]
    public class LightFilter : RangeFilter<string>, IFilterDisplay<string>
    {
        private IEnumerable<string> LightValues { get; set; }
        public LightFilter(RangeFilter<string> filter)
        {
            Name = filter.Name;
            InputType = filter.InputType;
            FilterType = filter.FilterType;
            MinimumValue = filter.MinimumValue;
            MaximumValue = filter.MaximumValue;
            LightValues = GetLightValues();
        }

        public LightFilter()
        {
            Name = "Light";
            InputType = InputType.SelectRange;
            FilterType = FilterType.String;
            var values = new List<string> { "" };
            values.AddRange(Enum.GetValues(typeof(LightType)).Cast<LightType>().Where(l => l != LightType.Unknown).Select(s => s.ToString()));
            FacetValues = values.ToDictionary(m => m, c => (long?)0L);
        }

        private IEnumerable<string> GetLightValues()
        {
            var lightValues = new List<string>();
            if (string.IsNullOrEmpty(MinimumValue) && string.IsNullOrEmpty(MaximumValue))
            {
                return lightValues;
            }

            if (string.IsNullOrEmpty(MinimumValue))
            {
                MinimumValue = LightType.Unknown.ToString();
            }
            if (string.IsNullOrEmpty(MaximumValue))
            {
                MaximumValue = LightType.Unknown.ToString();
            }

            if (Enum.TryParse<LightType>(MinimumValue, out var minValue) && Enum.TryParse<LightType>(MaximumValue, out var maxValue))
            {
                if (minValue != LightType.Unknown)
                {
                    lightValues.Add(minValue.ToString());
                }

                if (maxValue != LightType.Unknown)
                {
                    lightValues.Add(maxValue.ToString());
                }

                if (minValue == LightType.FullShade || minValue == LightType.Unknown)
                {
                    if (maxValue == LightType.FullSun || maxValue == LightType.Unknown)
                    {
                        lightValues.Add(LightType.PartShade.ToString());
                        lightValues.Add(LightType.PartSun.ToString());
                    }
                    else if (maxValue == LightType.PartSun)
                    {
                        lightValues.Add(LightType.PartShade.ToString());
                    }
                }
                else if (minValue == LightType.PartShade && maxValue == LightType.FullSun)
                {
                    lightValues.Add(LightType.PartSun.ToString());
                }
            }
            return lightValues;
        }

        public string DisplayValue(string value, long? count = null)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            else
            {
                var lightValue = Enum.Parse<LightType>(value);
                return count != null ? $"{lightValue.ToFriendlyName()} ({count})" : $"{lightValue.ToFriendlyName()}";
            }
        }
    }
}
