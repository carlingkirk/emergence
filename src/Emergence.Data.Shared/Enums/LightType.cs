using System.ComponentModel;

namespace Emergence.Data.Shared
{
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
        [Description("Full Shade")]
        FullShade
    }
}
