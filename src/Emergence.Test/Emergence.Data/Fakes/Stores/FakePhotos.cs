using System;
using System.Collections.Generic;
using Emergence.Data.Shared.Stores;

namespace Emergence.Test.Data.Fakes.Stores
{
    public static class FakePhotos
    {
        public static IEnumerable<Photo> Get()
        {
            var photos = new List<Photo>
            {
                new Photo
                {
                    Id = 1,
                    Type = "Activity",
                    TypeId = 1,
                    Filename = "1.png",
                    CreatedBy = "me",
                    LocationId = 1,
                    ContentType = "image/png",
                    Height = 1200,
                    Width = 1600,
                    BlobPath = Guid.NewGuid().ToString(),
                    DateTaken = new DateTime(2020, 07, 24, 15, 45, 12, DateTimeKind.Utc),
                    DateCreated = new DateTime(2020, 07, 24, 15, 45, 12, DateTimeKind.Utc),
                    DateModified = new DateTime(2020, 07, 24, 15, 45, 12, DateTimeKind.Utc)
                },
                new Photo
                {
                    Id = 2,
                    Type = "PlantInfo",
                    TypeId = 2,
                    Filename = "2.png",
                    CreatedBy = "me",
                    LocationId = 1,
                    ContentType = "image/png",
                    Height = 1200,
                    Width = 1600,
                    BlobPath = Guid.NewGuid().ToString(),
                    DateTaken = new DateTime(2020, 07, 24, 15, 45, 12, DateTimeKind.Utc),
                    DateCreated = new DateTime(2020, 07, 24, 15, 45, 12, DateTimeKind.Utc),
                    DateModified = new DateTime(2020, 07, 24, 15, 45, 12, DateTimeKind.Utc)
                },
                new Photo
                {
                    Id = 3,
                    Type = "Specimen",
                    TypeId = 3,
                    Filename = "3.png",
                    CreatedBy = "me",
                    LocationId = 1,
                    ContentType = "image/png",
                    Height = 1200,
                    Width = 1600,
                    BlobPath = Guid.NewGuid().ToString(),
                    DateTaken = new DateTime(2020, 07, 24, 15, 45, 12, DateTimeKind.Utc),
                    DateCreated = new DateTime(2020, 07, 24, 15, 45, 12, DateTimeKind.Utc),
                    DateModified = new DateTime(2020, 07, 24, 15, 45, 12, DateTimeKind.Utc)
                },
            };
            return photos;
        }
    }
}
