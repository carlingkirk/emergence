using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Search
{
    [TypeDiscriminator("Water")]
    public class WaterFilter : RangeFilter<string>, IFilterDisplay<string>
    {
        private IEnumerable<string> WaterValues { get; set; }
        public WaterFilter(RangeFilter<string> filter)
        {
            Name = filter.Name;
            InputType = filter.InputType;
            FilterType = filter.FilterType;
            MinimumValue = filter.MinimumValue;
            MaximumValue = filter.MaximumValue;
            WaterValues = GetWaterValues();
        }

        public WaterFilter()
        {
            Name = "Water";
            InputType = InputType.SelectRange;
            FilterType = FilterType.String;
            var values = new List<string> { "" };
            values.AddRange(Enum.GetValues(typeof(WaterType)).Cast<WaterType>().Where(l => l != WaterType.Unknown).Select(s => s.ToString()));
            Values = values;
        }

        public Expression<Func<PlantInfo, bool>> Filter => p =>
            (p.MinimumWater != null && WaterValues.Contains(p.MinimumWater)) ||
            (p.MaximumWater != null && WaterValues.Contains(p.MaximumWater));

        private IEnumerable<string> GetWaterValues()
        {
            var waterValues = new List<string>();
            if (string.IsNullOrEmpty(MinimumValue) && string.IsNullOrEmpty(MaximumValue))
            {
                return waterValues;
            }

            if (string.IsNullOrEmpty(MinimumValue))
            {
                MinimumValue = WaterType.Unknown.ToString();
            }
            if (string.IsNullOrEmpty(MaximumValue))
            {
                MaximumValue = WaterType.Unknown.ToString();
            }

            if (Enum.TryParse<WaterType>(MinimumValue, out var minValue) && Enum.TryParse<WaterType>(MaximumValue, out var maxValue))
            {
                if (minValue != WaterType.Unknown)
                {
                    waterValues.Add(minValue.ToString());
                }

                if (maxValue != WaterType.Unknown)
                {
                    waterValues.Add(maxValue.ToString());
                }

                if (minValue == WaterType.Dry || minValue == WaterType.Unknown)
                {
                    if (maxValue == WaterType.Wet || maxValue == WaterType.Unknown)
                    {
                        waterValues.Add(WaterType.MediumDry.ToString());
                        waterValues.Add(WaterType.Medium.ToString());
                        waterValues.Add(WaterType.MediumWet.ToString());
                    }
                    else if (maxValue == WaterType.Medium)
                    {
                        waterValues.Add(WaterType.MediumDry.ToString());
                    }
                    else if (maxValue == WaterType.MediumWet)
                    {
                        waterValues.Add(WaterType.MediumDry.ToString());
                        waterValues.Add(WaterType.Medium.ToString());
                    }
                }
                else if (minValue == WaterType.MediumDry)
                {
                    if (maxValue == WaterType.MediumWet)
                    {
                        waterValues.Add(WaterType.Medium.ToString());
                    }
                    if (maxValue == WaterType.Wet)
                    {
                        waterValues.Add(WaterType.MediumWet.ToString());
                    }
                }
                else if (minValue == WaterType.Medium && maxValue == WaterType.Wet)
                {
                    waterValues.Add(WaterType.MediumWet.ToString());
                }
            }
            return waterValues;
        }

        public string DisplayValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            else
            {
                var lightValue = Enum.Parse<WaterType>(value);
                return lightValue.ToFriendlyName();
            }
        }
    }
}
