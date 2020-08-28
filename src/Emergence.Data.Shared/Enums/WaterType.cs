using System.ComponentModel;

namespace Emergence.Data.Shared
{
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
}
