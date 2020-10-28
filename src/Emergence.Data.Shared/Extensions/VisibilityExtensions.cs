using System.Linq;
using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Extensions
{
    public static class VisibilityExtensions
    {
        public static bool CanView(this IVisibile content, Models.User user) =>
            content.Visibility != Visibility.Hidden &&
             content.User.ProfileVisibility != Visibility.Hidden &&
            (content.Visibility != Visibility.Contacts ||
             content.User.Contacts.Any(c => c.UserId == user.Id));

        public static IQueryable<Activity> CanViewContent(this IQueryable<Activity> content, Models.User user) =>
            content = content.Where(c => c.CanView(user) &&
                                         c.User.ActivityVisibility != Visibility.Hidden &&
                                         (c.User.ActivityVisibility != Visibility.Contacts ||
                                          c.User.Contacts.Any(c => c.UserId == user.Id)));

        public static IQueryable<InventoryItem> CanViewContent(this IQueryable<InventoryItem> content, Models.User user) =>
            content = content.Where(c => (c.Visibility != Visibility.Hidden &&
                                c.User.ProfileVisibility != Visibility.Hidden) ||
                               (c.Visibility == Visibility.Contacts &&
                                c.User.Contacts.Any(c => c.UserId == user.Id) &&
                                c.User.InventoryItemVisibility != Visibility.Hidden) ||
                                (c.User.InventoryItemVisibility == Visibility.Contacts &&
                                c.User.Contacts.Any(c => c.UserId == user.Id)));

        public static IQueryable<Specimen> CanViewContent(this IQueryable<Specimen> content, Models.User user) =>
            content = content.Where(c => c.InventoryItem.Visibility != Visibility.Hidden &&
                                         c.InventoryItem.User.ProfileVisibility != Visibility.Hidden &&
                                        (c.InventoryItem.Visibility != Visibility.Contacts ||
                                         c.InventoryItem.User.Contacts.Any(c => c.ContactUserId == user.Id)) &&
                                         c.InventoryItem.User.InventoryItemVisibility != Visibility.Hidden &&
                                        (c.InventoryItem.User.InventoryItemVisibility != Visibility.Contacts ||
                                         c.InventoryItem.User.Contacts.Any(c => c.ContactUserId == user.Id)));

        public static IQueryable<Origin> CanViewContent(this IQueryable<Origin> content, Models.User user) =>
            content = content.Where(c => (c.Visibility != Visibility.Hidden &&
                                c.User.ProfileVisibility != Visibility.Hidden) ||
                               (c.Visibility == Visibility.Contacts &&
                                c.User.Contacts.Any(c => c.UserId == user.Id) &&
                                c.User.OriginVisibility != Visibility.Hidden) ||
                               (c.User.OriginVisibility == Visibility.Contacts &&
                                c.User.Contacts.Any(c => c.UserId == user.Id)));

        public static IQueryable<PlantInfo> CanViewContent(this IQueryable<PlantInfo> content, Models.User user) =>
            content = content.Where(c => (c.Visibility != Visibility.Hidden &&
                                c.User.ProfileVisibility != Visibility.Hidden) ||
                               (c.Visibility == Visibility.Contacts &&
                                c.User.Contacts.Any(c => c.UserId == user.Id) &&
                                c.User.PlantInfoVisibility != Visibility.Hidden) ||
                               (c.User.PlantInfoVisibility == Visibility.Contacts &&
                                c.User.Contacts.Any(c => c.UserId == user.Id)));
    }
}
