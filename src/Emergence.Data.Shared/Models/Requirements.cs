using System.Collections.Generic;
using System.ComponentModel;

namespace Emergence.Data.Shared.Models
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
    }

    public class LightRequirements
    {
        public LightType MinimumLight { get; set; }
        public LightType MaximumLight { get; set; }
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
    }

    public class Zone
    {
        public int Number { get; set; }
        public string Letter { get; set; }
    }

    public enum WaterType
    {
        [Description("")]
        Unknown,
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
        [Description("")]
        Unknown,
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

    public enum StratificationType
    {
        [Description("Refrigerate seed for storage")]
        Refrigeration,
        [Description("Sand scarification")]
        AbrasionScarify,
        [Description("Nick scarification")]
        NickScarify,
        [Description("Hot water treatment")]
        HotWater,
        [Description("Cold moist stratification")]
        ColdMoist,
        [Description("Warm moist stratification")]
        WarmMoist,
        [Description("Needs light to germinate")]
        LightGermination,
        [Description("Sow in late fall")]
        CoolSoil,
        [Description("Sow outdoors in fall")]
        FallOutdoors,
        [Description("Requires inoculum")]
        Rhizobia,
        [Description("Requires host plant")]
        HostPlant,
        [Description("Fern spores")]
        FernSpores
    }
}
