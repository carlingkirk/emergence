using System.ComponentModel;

namespace Emergence.Data.Shared
{
    public enum ProfileVisibility
    {
        [Description("Public")]
        Public = 1,
        [Description("My Contacts")]
        Contacts = 2,
        [Description("Private")]
        Hidden = 3
    }

    public enum Visibility
    {
        [Description("Inherit from profile")]
        Inherit = 0,
        [Description("Public")]
        Public = 1,
        [Description("My Contacts")]
        Contacts = 2,
        [Description("Private")]
        Hidden = 3
    }
}
