using System.ComponentModel;

namespace Emergence.Data.Shared
{
    public enum DistanceUnit
    {
        [Description("")]
        Unknown,
        [Description("ft")]
        Feet,
        [Description("in")]
        Inches,
        [Description("m")]
        Meters,
        [Description("cm")]
        Centimeters
    }
}
