using System.ComponentModel;

namespace Emergence.Data.Shared
{
    public enum LightType
    {
        [Description("")]
        Unknown,
        [Description("Full Shade")]
        FullShade,
        [Description("Part Shade")]
        PartShade,
        [Description("Part Sun")]
        PartSun,
        [Description("Full Sun")]
        FullSun
    }
}
