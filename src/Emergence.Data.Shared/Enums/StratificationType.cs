using System.ComponentModel;

namespace Emergence.Data.Shared
{
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
