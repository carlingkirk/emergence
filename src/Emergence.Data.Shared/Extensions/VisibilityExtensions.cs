using System.Linq;
using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Extensions
{
    public static class VisibilityExtensions
    {
        public static IQueryable<Activity> CanViewContent(this IQueryable<Activity> content, Models.User user) =>
            content = content.Where(c => c.Visibility == Visibility.Public ||
                                         // Not hidden
                                         (c.Visibility != Visibility.Hidden &&
                                          c.Visibility == Visibility.Inherit &&
                                          c.User.ActivityVisibility != Visibility.Hidden &&
                                          c.User.ProfileVisibility != Visibility.Hidden) ||
                                         // Inherited
                                         (c.Visibility == Visibility.Inherit && //yes
                                          (c.User.ActivityVisibility == Visibility.Public || //no
                                           (c.User.ActivityVisibility == Visibility.Inherit && // yes
                                            c.User.ProfileVisibility == Visibility.Public) || // no
                                           (c.User.ActivityVisibility == Visibility.Contacts && // no
                                            c.User.Contacts != null &&
                                            c.User.Contacts.Any(c => c.ContactUserId == user.Id)))) ||
                                         // Contacts
                                         (c.Visibility == Visibility.Contacts &&
                                          c.User.Contacts != null &&
                                          c.User.Contacts.Any(c => c.ContactUserId == user.Id)));

        public static IQueryable<InventoryItem> CanViewContent(this IQueryable<InventoryItem> content, Models.User user) =>
            content = content.Where(c => c.Visibility == Visibility.Public ||
                                         // Not hidden
                                         (c.Visibility != Visibility.Hidden &&
                                          c.Visibility == Visibility.Inherit &&
                                          c.User.InventoryItemVisibility != Visibility.Hidden &&
                                          c.User.ProfileVisibility != Visibility.Hidden) ||
                                         // Inherited
                                         (c.Visibility == Visibility.Inherit && //yes
                                          (c.User.InventoryItemVisibility == Visibility.Public || //no
                                           (c.User.InventoryItemVisibility == Visibility.Inherit && // yes
                                            c.User.ProfileVisibility == Visibility.Public) || // no
                                           (c.User.InventoryItemVisibility == Visibility.Contacts && // no
                                            c.User.Contacts != null &&
                                            c.User.Contacts.Any(c => c.ContactUserId == user.Id)))) ||
                                         // Contacts
                                         (c.Visibility == Visibility.Contacts &&
                                          c.User.Contacts != null &&
                                          c.User.Contacts.Any(c => c.ContactUserId == user.Id)));

        public static IQueryable<Specimen> CanViewContent(this IQueryable<Specimen> content, Models.User user) =>
            content = content.Where(c => c.InventoryItem.Visibility == Visibility.Public ||
                                         // Not hidden
                                         (c.InventoryItem.Visibility != Visibility.Hidden &&
                                          c.InventoryItem.Visibility == Visibility.Inherit &&
                                          c.InventoryItem.User.InventoryItemVisibility != Visibility.Hidden &&
                                          c.InventoryItem.User.ProfileVisibility != Visibility.Hidden) ||
                                         // Inherited
                                         (c.InventoryItem.Visibility == Visibility.Inherit && //yes
                                          (c.InventoryItem.User.InventoryItemVisibility == Visibility.Public || //no
                                           (c.InventoryItem.User.InventoryItemVisibility == Visibility.Inherit && // yes
                                            c.InventoryItem.User.ProfileVisibility == Visibility.Public) || // no
                                           (c.InventoryItem.User.InventoryItemVisibility == Visibility.Contacts && // no
                                            c.InventoryItem.User.Contacts != null &&
                                            c.InventoryItem.User.Contacts.Any(c => c.ContactUserId == user.Id)))) ||
                                         // Contacts
                                         (c.InventoryItem.Visibility == Visibility.Contacts &&
                                          c.InventoryItem.User.Contacts != null &&
                                          c.InventoryItem.User.Contacts.Any(c => c.ContactUserId == user.Id)));

        public static IQueryable<Origin> CanViewContent(this IQueryable<Origin> content, Models.User user) =>
            content = content.Where(c => c.Visibility == Visibility.Public ||
                                         // Not hidden
                                         (c.Visibility != Visibility.Hidden &&
                                          c.Visibility == Visibility.Inherit &&
                                          c.User.OriginVisibility != Visibility.Hidden &&
                                          c.User.ProfileVisibility != Visibility.Hidden) ||
                                         // Inherited
                                         (c.Visibility == Visibility.Inherit && //yes
                                          (c.User.OriginVisibility == Visibility.Public || //no
                                           (c.User.OriginVisibility == Visibility.Inherit && // yes
                                            c.User.ProfileVisibility == Visibility.Public) || // no
                                           (c.User.OriginVisibility == Visibility.Contacts && // no
                                            c.User.Contacts != null &&
                                            c.User.Contacts.Any(c => c.ContactUserId == user.Id)))) ||
                                         // Contacts
                                         (c.Visibility == Visibility.Contacts &&
                                          c.User.Contacts != null &&
                                          c.User.Contacts.Any(c => c.ContactUserId == user.Id)));

        public static IQueryable<PlantInfo> CanViewContent(this IQueryable<PlantInfo> content, Models.User user) =>
            content = content.Where(c => c.Visibility == Visibility.Public ||
                                         // Not hidden
                                         (c.Visibility != Visibility.Hidden &&
                                          c.Visibility == Visibility.Inherit &&
                                          c.User.PlantInfoVisibility != Visibility.Hidden &&
                                          c.User.ProfileVisibility != Visibility.Hidden) ||
                                         // Inherited
                                         (c.Visibility == Visibility.Inherit && //yes
                                          (c.User.PlantInfoVisibility == Visibility.Public || //no
                                           (c.User.PlantInfoVisibility == Visibility.Inherit && // yes
                                            c.User.ProfileVisibility == Visibility.Public) || // no
                                           (c.User.PlantInfoVisibility == Visibility.Contacts && // no
                                            c.User.Contacts != null &&
                                            c.User.Contacts.Any(c => c.ContactUserId == user.Id)))) ||
                                         // Contacts
                                         (c.Visibility == Visibility.Contacts &&
                                          c.User.Contacts != null &&
                                          c.User.Contacts.Any(c => c.ContactUserId == user.Id)));
    }
}
