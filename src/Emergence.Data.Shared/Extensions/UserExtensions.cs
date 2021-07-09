using System.Linq;
using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Extensions
{
    public static class UserExtensions
    {
        public static Models.User AsModel(this User source) => new Models.User
        {
            Id = source.Id,
            UserId = source.UserId,
            DisplayName = source.DisplayName,
            FirstName = source.FirstName,
            LastName = source.LastName,
            Bio = source.Bio,
            Location = source.Location != null ? source.Location.AsModel() : source.LocationId.HasValue ? new Models.Location { LocationId = source.LocationId.Value } : null,
            Photo = source.Photo != null ? source.Photo.AsModel() : source.PhotoId.HasValue ? new Models.Photo { PhotoId = source.PhotoId.Value } : null,
            EmailUpdates = source.EmailUpdates,
            SocialUpdates = source.SocialUpdates,
            ProfileVisibility = source.ProfileVisibility,
            InventoryItemVisibility = source.InventoryItemVisibility,
            PlantInfoVisibility = source.PlantInfoVisibility,
            OriginVisibility = source.OriginVisibility,
            ActivityVisibility = source.ActivityVisibility,
            DateCreated = source.DateCreated,
            DateModified = source.DateModified
        };

        public static Models.UserSummary AsSummaryModel(this User source) => new Models.UserSummary
        {
            Id = source.Id,
            DisplayName = source.DisplayName,
            PhotoId = source.Photo?.Id,
            PhotoThumbnailUri = source.Photo?.AsModel().ThumbnailUri,
            Visibility = source.ProfileVisibility
        };

        public static Models.UserSummary AsSummaryModel(this Models.User source) => new Models.UserSummary
        {
            Id = source.Id,
            DisplayName = source.DisplayName,
            PhotoId = source.Photo?.PhotoId,
            PhotoThumbnailUri = source.Photo?.ThumbnailUri,
            Visibility = source.ProfileVisibility
        };

        public static User AsStore(this Models.User source) => new User
        {
            Id = source.Id,
            UserId = source.UserId,
            DisplayName = source.DisplayName,
            FirstName = source.FirstName,
            LastName = source.LastName,
            Bio = source.Bio,
            LocationId = source.Location?.LocationId,
            PhotoId = source.Photo?.PhotoId,
            EmailUpdates = source.EmailUpdates,
            SocialUpdates = source.SocialUpdates,
            ProfileVisibility = source.ProfileVisibility,
            InventoryItemVisibility = source.InventoryItemVisibility,
            PlantInfoVisibility = source.PlantInfoVisibility,
            OriginVisibility = source.OriginVisibility,
            ActivityVisibility = source.ActivityVisibility,
            DateCreated = source.DateCreated,
            DateModified = source.DateModified
        };

        public static Search.Models.User AsSearchModel(this User source) => new Search.Models.User
        {
            Id = source.Id,
            UserId = source.UserId,
            DisplayName = source.DisplayName,
            FirstName = source.FirstName,
            LastName = source.LastName,
            Bio = source.Bio,
            Location = source.Location?.AsSearchModel(),
            Photo = source.Photo?.AsSearchModel(),
            EmailUpdates = source.EmailUpdates,
            SocialUpdates = source.SocialUpdates,
            ProfileVisibility = source.ProfileVisibility,
            InventoryItemVisibility = source.InventoryItemVisibility,
            PlantInfoVisibility = source.PlantInfoVisibility,
            OriginVisibility = source.OriginVisibility,
            ActivityVisibility = source.ActivityVisibility,
            DateCreated = source.DateCreated,
            DateModified = source.DateModified,
            ContactIds = source.Contacts?.Select(c => c.Id)
        };

        public static Search.Models.User AsSearchModel(this Models.UserSummary source) => new Search.Models.User
        {
            Id = source.Id,
            DisplayName = source.DisplayName,
            ProfileVisibility = source.Visibility
        };
    }
}
