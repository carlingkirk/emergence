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
            SpecimenVisibility = source.SpecimenVisibility,
            PlantInfoVisibility = source.PlantInfoVisibility,
            OriginVisibility = source.OriginVisibility,
            ActivityVisibility = source.ActivityVisibility,
            DateCreated = source.DateCreated,
            DateModified = source.DateModified
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
            SpecimenVisibility = source.SpecimenVisibility,
            PlantInfoVisibility = source.PlantInfoVisibility,
            OriginVisibility = source.OriginVisibility,
            ActivityVisibility = source.ActivityVisibility,
            DateCreated = source.DateCreated,
            DateModified = source.DateModified
        };
    }
}
