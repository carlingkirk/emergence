using System.Collections.Generic;
using Emergence.Data.Shared.Extensions;

namespace Emergence.Data.Shared
{
    public class Requirements
    {
        public WaterRequirements WaterRequirements { get; set; }
        public LightRequirements LightRequirements { get; set; }
        public IEnumerable<SoilType> SoilRequirements { get; set; }
        public List<StratificationStage> StratificationStages { get; set; }
        public ZoneRequirements ZoneRequirements { get; set; }
    }

    public class WaterRequirements
    {
        public WaterType MinimumWater { get; set; }
        public WaterType MaximumWater { get; set; }

        public string ToFriendlyString()
        {
            if (MinimumWater != WaterType.Unknown && MinimumWater != WaterType.Unknown)
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
        public LightType MinimumLight { get; set; }
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
        public StratificationType StratificationType { get; set; }
    }

    public class ZoneRequirements
    {
        public Zone MinimumZone { get; set; }
        public Zone MaximumZone { get; set; }

        public string ToFriendlyString()
        {
            if (!string.IsNullOrEmpty(MinimumZone?.ToFriendlyString()) && !string.IsNullOrEmpty(MaximumZone?.ToFriendlyString()))
            {
                return MinimumZone.ToFriendlyString() + " - " + MaximumZone.ToFriendlyString();
            }
            else if (!string.IsNullOrEmpty(MinimumZone?.ToFriendlyString()))
            {
                return MinimumZone.ToFriendlyString();
            }
            else if (!string.IsNullOrEmpty(MaximumZone?.ToFriendlyString()))
            {
                return MaximumZone.ToFriendlyString();
            }
            return null;
        }
    }

    public class Zone
    {
        public int Number { get; set; }
        public string Letter { get; set; }

        public string ToFriendlyString()
        {
            if (Number >= 0 && !string.IsNullOrEmpty(Letter))
            {
                return Number.ToString() + Letter;
            }
            else if (Number >= 0)
            {
                return Number.ToString();
            }
            else
            {
                return null;
            }
        }
    }

    public class BloomTime
    {
        public Month MinimumBloomTime { get; set; }
        public Month MaximumBloomTime { get; set; }

        public string ToFriendlyString()
        {
            if (MinimumBloomTime != Month.Unknown && MaximumBloomTime != Month.Unknown)
            {
                return MinimumBloomTime.ToFriendlyName() + " - " + MaximumBloomTime.ToFriendlyName();
            }
            else if (MinimumBloomTime != Month.Unknown)
            {
                return MinimumBloomTime.ToFriendlyName();
            }
            else if (MaximumBloomTime != Month.Unknown)
            {
                return MaximumBloomTime.ToFriendlyName();
            }
            return null;
        }
    }

    public class Height
    {
        public double? MinimumHeight { get; set; }
        public double? MaximumHeight { get; set; }
        public DistanceUnit Unit { get; set; }

        public string ToFriendlyString()
        {
            string height = null;
            if (MinimumHeight != null && MaximumHeight != null)
            {
                height = string.Format("{0:0.00}", MinimumHeight) + " - " + string.Format("{0:0.00}", MaximumHeight);
            }
            else if (MinimumHeight != null)
            {
                height = string.Format("{0:0.00}", MinimumHeight);
            }
            else if (MaximumHeight != null)
            {
                height = string.Format("{0:0.00}", MaximumHeight);
            }

            if (Unit != DistanceUnit.Unknown)
            {
                height = height + " " + Unit.ToFriendlyName();
            }

            return height;
        }
    }

    public class Spread
    {
        public double? MinimumSpread { get; set; }
        public double? MaximumSpread { get; set; }
        public DistanceUnit Unit { get; set; }

        public string ToFriendlyString()
        {
            string height = null;
            if (MinimumSpread != null && MaximumSpread != null)
            {
                height = string.Format("{0:0.00}", MinimumSpread) + " - " + string.Format("{0:0.00}", MaximumSpread);
            }
            else if (MinimumSpread != null)
            {
                height = string.Format("{0:0.00}", MinimumSpread);
            }
            else if (MaximumSpread != null)
            {
                height = string.Format("{0:0.00}", MaximumSpread);
            }

            if (Unit != DistanceUnit.Unknown)
            {
                height = height + " " + Unit.ToFriendlyName();
            }

            return height;
        }
    }
}

