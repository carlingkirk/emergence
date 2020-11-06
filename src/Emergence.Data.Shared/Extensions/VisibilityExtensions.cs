using System.Linq;
using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Extensions
{
    public static class VisibilityExtensions
    {
        public static IQueryable<Activity> CanViewContent(this IQueryable<Activity> content, Models.User user) =>
            content = content.Where(c => c.UserId == user.Id || c.Visibility == Visibility.Public ||
                                         // Not hidden
                                         (!(c.Visibility == Visibility.Hidden ||
                                            (c.Visibility == Visibility.Inherit &&
                                             c.User.ActivityVisibility == Visibility.Hidden) ||
                                            (c.User.ActivityVisibility == Visibility.Inherit &&
                                             c.User.ProfileVisibility == Visibility.Hidden)) &&
                                         // Inherited
                                         ((c.Visibility == Visibility.Inherit &&
                                          (c.User.ActivityVisibility == Visibility.Public ||
                                           (c.User.ActivityVisibility == Visibility.Inherit &&
                                            c.User.ProfileVisibility == Visibility.Public) ||
                                           (c.User.ActivityVisibility == Visibility.Contacts &&
                                            c.User.Contacts != null &&
                                            c.User.Contacts.Any(c => c.ContactUserId == user.Id)))) ||
                                         // Contacts
                                         (c.Visibility == Visibility.Contacts &&
                                          c.User.Contacts != null &&
                                          c.User.Contacts.Any(c => c.ContactUserId == user.Id)))));

        public static IQueryable<InventoryItem> CanViewContent(this IQueryable<InventoryItem> content, Models.User user) =>
            content = content.Where(c => c.UserId == user.Id || c.Visibility == Visibility.Public ||
                                         // Not hidden
                                         (!(c.Visibility == Visibility.Hidden ||
                                            (c.Visibility == Visibility.Inherit &&
                                             c.User.InventoryItemVisibility == Visibility.Hidden) ||
                                            (c.User.InventoryItemVisibility == Visibility.Inherit &&
                                             c.User.ProfileVisibility == Visibility.Hidden)) &&
                                         // Inherited
                                         ((c.Visibility == Visibility.Inherit &&
                                          (c.User.InventoryItemVisibility == Visibility.Public ||
                                           (c.User.InventoryItemVisibility == Visibility.Inherit &&
                                            c.User.ProfileVisibility == Visibility.Public) ||
                                           (c.User.InventoryItemVisibility == Visibility.Contacts &&
                                            c.User.Contacts != null &&
                                            c.User.Contacts.Any(c => c.ContactUserId == user.Id)))) ||
                                         // Contacts
                                         (c.Visibility == Visibility.Contacts &&
                                          c.User.Contacts != null &&
                                          c.User.Contacts.Any(c => c.ContactUserId == user.Id)))));

        public static IQueryable<Specimen> CanViewContent(this IQueryable<Specimen> content, Models.User user) =>
            content = content.Where(c => c.InventoryItem.UserId == user.Id || c.InventoryItem.Visibility == Visibility.Public ||
                                         // Not hidden
                                         (!(c.InventoryItem.Visibility == Visibility.Hidden ||
                                            (c.InventoryItem.Visibility == Visibility.Inherit &&
                                             c.InventoryItem.User.InventoryItemVisibility == Visibility.Hidden) ||
                                            (c.InventoryItem.User.InventoryItemVisibility == Visibility.Inherit &&
                                             c.InventoryItem.User.ProfileVisibility == Visibility.Hidden)) &&
                                         // Inherited
                                         ((c.InventoryItem.Visibility == Visibility.Inherit &&
                                          (c.InventoryItem.User.InventoryItemVisibility == Visibility.Public ||
                                           (c.InventoryItem.User.InventoryItemVisibility == Visibility.Inherit &&
                                            c.InventoryItem.User.ProfileVisibility == Visibility.Public) ||
                                           (c.InventoryItem.User.InventoryItemVisibility == Visibility.Contacts &&
                                            c.InventoryItem.User.Contacts != null &&
                                            c.InventoryItem.User.Contacts.Any(c => c.ContactUserId == user.Id)))) ||
                                         // Contacts
                                         (c.InventoryItem.Visibility == Visibility.Contacts &&
                                          c.InventoryItem.User.Contacts != null &&
                                          c.InventoryItem.User.Contacts.Any(c => c.ContactUserId == user.Id)))));

        public static IQueryable<Origin> CanViewContent(this IQueryable<Origin> content, Models.User user) =>
            content = content.Where(c => c.UserId == user.Id || c.Visibility == Visibility.Public ||
                                         // Not hidden
                                         (!(c.Visibility == Visibility.Hidden ||
                                            (c.Visibility == Visibility.Inherit &&
                                             c.User.OriginVisibility == Visibility.Hidden) ||
                                            (c.User.OriginVisibility == Visibility.Inherit &&
                                             c.User.ProfileVisibility == Visibility.Hidden)) &&
                                         // Inherited
                                         ((c.Visibility == Visibility.Inherit &&
                                          (c.User.OriginVisibility == Visibility.Public ||
                                           (c.User.OriginVisibility == Visibility.Inherit &&
                                            c.User.ProfileVisibility == Visibility.Public) ||
                                           (c.User.OriginVisibility == Visibility.Contacts &&
                                            c.User.Contacts != null &&
                                            c.User.Contacts.Any(c => c.ContactUserId == user.Id)))) ||
                                         // Contacts
                                         (c.Visibility == Visibility.Contacts &&
                                          c.User.Contacts != null &&
                                          c.User.Contacts.Any(c => c.ContactUserId == user.Id)))));

        public static IQueryable<PlantInfo> CanViewContent(this IQueryable<PlantInfo> content, Models.User user) =>
            content = content.Where(c => c.UserId == user.Id || c.Visibility == Visibility.Public ||
                                         // Not hidden
                                         (!(c.Visibility == Visibility.Hidden ||
                                            (c.Visibility == Visibility.Inherit &&
                                             c.User.PlantInfoVisibility == Visibility.Hidden) ||
                                            (c.User.PlantInfoVisibility == Visibility.Inherit &&
                                             c.User.ProfileVisibility == Visibility.Hidden)) &&
                                         // Inherited
                                         ((c.Visibility == Visibility.Inherit &&
                                          (c.User.PlantInfoVisibility == Visibility.Public ||
                                           (c.User.PlantInfoVisibility == Visibility.Inherit &&
                                            c.User.ProfileVisibility == Visibility.Public) ||
                                           (c.User.PlantInfoVisibility == Visibility.Contacts &&
                                            c.User.Contacts != null &&
                                            c.User.Contacts.Any(c => c.ContactUserId == user.Id)))) ||
                                         // Contacts
                                         (c.Visibility == Visibility.Contacts &&
                                          c.User.Contacts != null &&
                                          c.User.Contacts.Any(c => c.ContactUserId == user.Id)))));

        public static IQueryable<User> CanViewUsers(this IQueryable<User> users, Models.User user) =>
            users = users.Where(u => u.UserId == user.UserId || u.ProfileVisibility == Visibility.Public ||
                                     // Contacts
                                     (u.ProfileVisibility == Visibility.Contacts &&
                                      u.Contacts != null &&
                                      u.Contacts.Any(c => c.ContactUserId == user.Id)));

        public static bool CanViewUser(this Models.User requestor, User user) =>
            user.UserId == requestor.UserId || user.ProfileVisibility == Visibility.Public ||
            // Contacts
            (user.ProfileVisibility == Visibility.Contacts &&
             user.Contacts != null &&
             user.Contacts.Any(c => c.ContactUserId == requestor.Id));
    }
}
