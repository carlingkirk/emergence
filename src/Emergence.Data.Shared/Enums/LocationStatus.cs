using System.ComponentModel;

namespace Emergence.Data.Shared
{
    public enum LocationStatus
    {
        [Description("")]
        Unknown,
        Native,
        Introduced,
        Incidental,
        [Description("Native & Introduced")]
        NativeIntroduced,
        [Description("Native & Extirpated")]
        NativeExtirpated
    }
}
