using System;
using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Extensions
{
    public static class PhotoExtensions
    {
        public static Models.Photo AsModel(this Photo source, string blobPathRoot = null) => new Models.Photo
        {
            PhotoId = source.Id,
            Type = Enum.Parse<PhotoType>(source.Type),
            TypeId = source.TypeId,
            Filename = source.Filename,
            BlobPath = source.BlobPath,
            BlobPathRoot = blobPathRoot,
            ExternalUrl = source.ExternalUrl,
            Location = source.Location != null ? source.Location.AsModel() : source.LocationId.HasValue ? new Models.Location { LocationId = source.LocationId.Value } : null,
            ContentType = source.ContentType,
            Height = source.Height,
            Width = source.Width,
            DateTaken = source.DateTaken,
            CreatedBy = source.CreatedBy,
            ModifiedBy = source.ModifiedBy,
            DateCreated = source.DateCreated,
            DateModified = source.DateModified
        };

        public static Photo AsStore(this Models.Photo source) => new Photo
        {
            Id = source.PhotoId,
            Type = source.Type.ToString(),
            TypeId = source.TypeId,
            Filename = source.Filename,
            BlobPath = source.BlobPath,
            ExternalUrl = source.ExternalUrl,
            LocationId = source.Location?.LocationId,
            ContentType = source.ContentType,
            Height = source.Height,
            Width = source.Width,
            DateTaken = source.DateTaken,
            CreatedBy = source.CreatedBy,
            ModifiedBy = source.ModifiedBy,
            DateCreated = source.DateCreated ?? DateTime.UtcNow,
            DateModified = source.DateModified
        };

        public static Search.Models.Photo AsSearchModel(this Photo source) => new Search.Models.Photo
        {
            Id = source.Id,
            Type = source.Type.ToString(),
            TypeId = source.TypeId,
            Filename = source.Filename,
            BlobPath = source.BlobPath,
            ExternalUrl = source.ExternalUrl,
            Location = source.Location?.AsSearchModel(),
            ContentType = source.ContentType,
            Height = source.Height,
            Width = source.Width,
            DateTaken = source.DateTaken,
            CreatedBy = source.CreatedBy,
            ModifiedBy = source.ModifiedBy,
            DateCreated = source.DateCreated,
            DateModified = source.DateModified
        };
    }
}
