using System.Collections.Generic;
using System.Text.Json.Serialization;
using Emergence.Data.Shared.Enums;
using Emergence.Data.Shared.Extensions;

namespace Emergence.Data.Shared.Models
{
    public class Requirements
    {
        public WaterRequirements WaterRequirements { get; set; }
        public LightRequirements LightRequirements { get; set; }
        public List<StratificationStage> StratificationStages { get; set; }
        public ZoneRequirements ZoneRequirements { get; set; }
    }

    public class WaterRequirements
    {
        [JsonConverter(typeof(EnumDisplayConverter<WaterType>))]
        public WaterType MinimumWater { get; set; }
        [JsonConverter(typeof(EnumDisplayConverter<WaterType>))]
        public WaterType MaximumWater { get; set; }

        public string ToFriendlyString()
        {
            if (MinimumWater != WaterType.Unknown && MaximumWater != WaterType.Unknown)
            {
                return MinimumWater.ToFriendlyName() + " - " + MaximumWater.ToFriendlyName();
            }
            else if (MinimumWater != WaterType.Unknown)
            {
                return MinimumWater.ToFriendlyName();
            }
            else if (MaximumWater != WaterType.Unknown)
            {
                return MaximumWater.ToFriendlyName();
            }
            return null;
        }
    }

    public class LightRequirements
    {
        [JsonConverter(typeof(EnumDisplayConverter<LightType>))]
        public LightType MinimumLight { get; set; }
        [JsonConverter(typeof(EnumDisplayConverter<LightType>))]
        public LightType MaximumLight { get; set; }

        public string ToFriendlyString()
        {
            if (MinimumLight != LightType.Unknown && MaximumLight != LightType.Unknown)
            {
                return MinimumLight.ToFriendlyName() + " - " + MaximumLight.ToFriendlyName();
            }
            else if (MinimumLight != LightType.Unknown)
            {
                return MinimumLight.ToFriendlyName();
            }
            else if (MaximumLight != LightType.Unknown)
            {
                return MaximumLight.ToFriendlyName();
            }
            return null;
        }
    }

    public class StratificationStage
    {
        public int Step { get; set; }
        public short DayLength { get; set; }
        [JsonConverter(typeof(EnumDisplayConverter<StratificationType>))]
        public StratificationType StratificationType { get; set; }
    }

    public class ZoneRequirements
    {
        public Zone MinimumZone { get; set; }
        public Zone MaximumZone { get; set; }

        public string ToFriendlyString()
        {
            if (MinimumZone != null && MaximumZone != null)
            {
                return MinimumZone.Name + " - " + MaximumZone.Name;
            }
            else if (MinimumZone != null)
            {
                return MinimumZone.Name;
            }
            else if (MaximumZone != null)
            {
                return MaximumZone.Name;
            }
            return null;
        }
    }
}

