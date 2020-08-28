using System.ComponentModel;

namespace Emergence.Data.Shared
{
    public enum OriginType
    {
        [Description("")]
        Unknown,
        Nursery,
        Store,
        Location,
        Person,
        Event,
        Website,
        Webpage,
        Publication,
        File
    }
}
