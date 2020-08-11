using System;
using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Extensions
{
    public static class PhotoExtensions
    {
        public static Models.Photo AsModel(this Photo source) => new Models.Photo
        {
            PhotoId = source.Id,
            Type = Enum.Parse<Models.PhotoType>(source.Type),
            TypeId = source.TypeId,
            Filename = source.Filename,
            UserId = source.UserId,
            Location = source.Location != null ? source.Location.AsModel() : source.LocationId.HasValue ? new Models.Location { LocationId = source.LocationId.Value } : null,
            ContentType = source.ContentType,
            Height = source.Height,
            Width = source.Width,
            DateTaken = source.DateTaken,
            DateCreated = source.DateCreated,
            DateModified = source.DateModified
        };

        public static Photo AsStore(this Models.Photo source) => new Photo
        {
            Id = source.PhotoId,
            Type = source.Type.ToString(),
            TypeId = source.TypeId,
            Filename = source.Filename,
            UserId = source.UserId,
            LocationId = source.Location?.LocationId,
            ContentType = source.ContentType,
            Height = source.Height,
            Width = source.Width,
            DateTaken = source.DateTaken,
            DateCreated = source.DateCreated ?? DateTime.UtcNow,
            DateModified = source.DateModified
        };
    }
}
