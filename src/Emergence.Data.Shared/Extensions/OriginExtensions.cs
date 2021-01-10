using System;
using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Extensions
{
    public static class OriginExtensions
    {
        public static Models.Origin AsModel(this Origin source) => new Models.Origin
        {
            OriginId = source.Id,
            ParentOrigin = source.ParentOrigin != null ? source.ParentOrigin.AsModel() : source.ParentOriginId.HasValue ? new Models.Origin { OriginId = source.ParentOriginId.Value } : null,
            Name = source.Name,
            Description = source.Description,
            Type = Enum.Parse<OriginType>(source.Type),
            LocationId = source.LocationId,
            Location = source.Location != null ? source.Location.AsModel() : source.LocationId.HasValue ? new Models.Location { LocationId = source.LocationId.Value } : null,
            Uri = source.Uri,
            Authors = source.Authors,
            ExternalId = source.ExternalId,
            AltExternalId = source.AltExternalId,
            Visibility = source.Visibility,
            UserId = source.UserId,
            User = source.User?.AsSummaryModel(),
            CreatedBy = source.CreatedBy,
            ModifiedBy = source.ModifiedBy,
            DateCreated = source.DateCreated,
            DateModified = source.DateModified
        };

        public static Origin AsStore(this Models.Origin source) => new Origin
        {
            Id = source.OriginId,
            ParentOriginId = source.ParentOrigin != null ? (int?)source.ParentOrigin.OriginId : null,
            LocationId = source.Location?.LocationId ?? source.LocationId,
            Name = source.Name,
            Description = source.Description,
            Type = source.Type.ToString(),
            Latitude = source.Location?.Latitude,
            Longitude = source.Location?.Longitude,
            Uri = source.Uri,
            Authors = source.Authors,
            ExternalId = source.ExternalId,
            AltExternalId = source.AltExternalId,
            Visibility = source.Visibility,
            UserId = source.UserId,
            CreatedBy = source.CreatedBy,
            ModifiedBy = source.ModifiedBy,
            DateCreated = source.DateCreated ?? DateTime.UtcNow,
            DateModified = source.DateModified
        };

        public static Search.Models.Origin AsSearchModel(this Origin source) => new Search.Models.Origin
        {
            Id = source.Id,
            ParentOrigin = source.ParentOrigin?.AsSearchModel(),
            Location = source.Location?.AsSearchModel(),
            Name = source.Name,
            Description = source.Description,
            Type = source.Type.ToString(),
            Latitude = source.Location?.Latitude,
            Longitude = source.Location?.Longitude,
            Uri = source.Uri?.ToString(),
            Authors = source.Authors,
            ExternalId = source.ExternalId,
            AltExternalId = source.AltExternalId,
            Visibility = source.Visibility,
            User = source.User?.AsSearchModel(),
            CreatedBy = source.CreatedBy,
            ModifiedBy = source.ModifiedBy,
            DateCreated = source.DateCreated,
            DateModified = source.DateModified
        };
    }
}
