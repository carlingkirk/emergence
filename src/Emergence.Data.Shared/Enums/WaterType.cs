using System.ComponentModel;

namespace Emergence.Data.Shared
{
    public enum WaterType
    {
        [Description("")]
        Unknown,
        Dry,
        [Description("Medium Dry")]
        MediumDry,
        Medium,
        [Description("Medium Wet")]
        MediumWet,
        Wet
    }
}
