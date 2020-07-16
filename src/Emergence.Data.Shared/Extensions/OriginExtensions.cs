using System;
using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Extensions
{
    public static class OriginExtensions
    {
        public static Models.Origin AsModel(this Origin source) => new Models.Origin
        {
            OriginId = source.Id,
            ParentOrigin = source.ParentId.HasValue ? new Models.Origin { OriginId = source.ParentId.Value } : null,
            Name = source.Name,
            Description = source.Description,
            Type = Enum.Parse<Models.OriginType>(source.Type),
            Location = new Models.Location { Latitude = source.Latitude, Longitude = source.Longitude },
            Uri = source.Uri,
            Authors = source.Authors,
            ExternalId = source.ExternalId,
            AltExternalId = source.AltExternalId,
            UserId = source.UserId,
            DateCreated = source.DateCreated,
            DateModified = source.DateModified
        };

        public static Origin AsStore(this Models.Origin source) => new Origin
        {
            Id = source.OriginId,
            ParentId = source.ParentOrigin != null ? (int?)source.ParentOrigin.OriginId : null,
            Name = source.Name,
            Description = source.Description,
            Type = source.Type.ToString(),
            Latitude = source.Location?.Latitude,
            Longitude = source.Location?.Longitude,
            Uri = source.Uri,
            Authors = source.Authors,
            ExternalId = source.ExternalId,
            AltExternalId = source.AltExternalId,
            UserId = source.UserId,
            DateCreated = source.DateCreated,
            DateModified = source.DateModified
        };
    }
}
