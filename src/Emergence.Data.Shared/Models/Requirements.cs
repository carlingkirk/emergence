using System.Collections.Generic;
using System.ComponentModel;

namespace Emergence.Data.Shared.Models
{
    public class Requirements
    {
        public WaterRequirements WaterRequirements { get; set; }
        public LightRequirements LightRequirements { get; set; }
        public IEnumerable<SoilType> SoilRequirements { get; set; }
        public StratificationRequirements StratificationRequirements { get; set; }
        public ScarificationRequirements ScarificationRequirements { get; set; }
        public SeedStorageRequirements SeedStorageRequirements { get; set; }
        public ZoneRequirements ZoneRequirements { get; set; }
    }

    public class WaterRequirements
    {
        public WaterType MinimumWater { get; set; }
        public WaterType MaximumWater { get; set; }
    }

    public class LightRequirements
    {
        public LightType MinimumLight { get; set; }
        public LightType MaximumLight { get; set; }
    }

    public class SeedStorageRequirements
    {
        public bool Refrigerate { get; set; }
    }

    public class StratificationRequirements
    {
        public IDictionary<int, StratificationStage> StratificationStages { get; set; }
    }

    public class ScarificationRequirements
    {
        public IEnumerable<ScarificationType> ScarificationTypes { get; set; }
    }

    public class StratificationStage
    {
        public short DayLength { get; set; }
        public short MinimumTemperature { get; set; }
        public short MaximumTemperature { get; set; }
    }

    public class ZoneRequirements
    {
        public Zone MinimumZone { get; set; }
        public Zone MaximumZone { get; set; }
    }

    public class Zone
    {
        public int Number { get; set; }
        public string Letter { get; set; }
    }

    public enum WaterType
    {
        Wet,
        [Description("Medium Wet")]
        MediumWet,
        Medium,
        [Description("Medium Dry")]
        MediumDry,
        Dry
    }

    public enum LightType
    {
        [Description("Full Sun")]
        FullSun,
        [Description("Part Sun")]
        PartSun,
        [Description("Part Shade")]
        PartShade,
        [Description("Full Sun")]
        FullShade
    }

    public enum SoilType
    {
        Fertile,
        Loamy,
        Rocky,
        Clay,
        Peaty,
        Swamp,
        Water
    }

    public enum ScarificationType
    {
        Sand,
        Nick
    }
}
