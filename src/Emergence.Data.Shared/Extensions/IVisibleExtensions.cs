using System.Linq;
using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Extensions
{
    public static class IVisibleExtensions
    {
        public static bool CanView(this IVisibile content, User user)
            => (content.Visibility != Visibility.Hidden &&
                content.User.ProfileVisibility != Visibility.Hidden) ||
                (content.Visibility == Visibility.Contacts &&
                content.User.Contacts.Any(c => c.UserId == user.Id));

        public static bool CanViewContent(this IVisibile<InventoryItem> content, User user)
            => (content.CanView(user) &&
                content.User.InventoryItemVisibility != Visibility.Hidden) ||
                (content.User.InventoryItemVisibility == Visibility.Contacts &&
                content.User.Contacts.Any(c => c.UserId == user.Id));

        public static bool CanViewContent(this IVisibile<Activity> content, User user)
            => (content.CanView(user) &&
                content.User.ActivityVisibility != Visibility.Hidden) ||
                (content.User.ActivityVisibility == Visibility.Contacts &&
                content.User.Contacts.Any(c => c.UserId == user.Id));

        public static bool CanViewContent(this IVisibile<Origin> content, User user)
            => (content.CanView(user) &&
                content.User.OriginVisibility != Visibility.Hidden) ||
                (content.User.OriginVisibility == Visibility.Contacts &&
                content.User.Contacts.Any(c => c.UserId == user.Id));

        public static bool CanViewContent(this IVisibile<PlantInfo> content, User user)
            => (content.CanView(user) &&
                content.User.PlantInfoVisibility != Visibility.Hidden) ||
                (content.User.PlantInfoVisibility == Visibility.Contacts &&
                content.User.Contacts.Any(c => c.UserId == user.Id));
    }
}
