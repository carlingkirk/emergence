using System.ComponentModel;

namespace Emergence.Data.Shared
{
    public enum ItemStatus
    {
        Available,
        Wishlist,
        Ordered,
        [Description("In Use")]
        InUse
    }
}
