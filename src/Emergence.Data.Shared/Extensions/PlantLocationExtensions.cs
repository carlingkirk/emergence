using System;
using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Extensions
{
    public static class PlantLocationExtensions
    {
        public static Models.PlantLocation AsModel(this PlantLocation source) => new Models.PlantLocation
        {
            Id = source.Id,
            Status = Enum.Parse<LocationStatus>(source.Status),
            Location = source.Location != null ? source.Location.AsModel() : new Models.Location { LocationId = source.LocationId },
            PlantInfo = source.PlantInfo != null ? source.PlantInfo.AsModel() : new Models.PlantInfo { PlantInfoId = source.PlantInfoId },
            CreatedBy = source.CreatedBy,
            ModifiedBy = source.ModifiedBy,
            DateCreated = source.DateCreated,
            DateModified = source.DateModified
        };

        public static PlantLocation AsStore(this Models.PlantLocation source) => new PlantLocation
        {
            Id = source.Id,
            Status = source.Status.ToString(),
            LocationId = source.Location.LocationId,
            PlantInfoId = source.PlantInfo.PlantInfoId,
            CreatedBy = source.CreatedBy,
            ModifiedBy = source.ModifiedBy,
            DateCreated = source.DateCreated ?? DateTime.UtcNow,
            DateModified = source.DateModified
        };

        public static Search.Models.PlantLocation AsSearchModel(this PlantLocation source) => new Search.Models.PlantLocation
        {
            Id = source.Id,
            Status = Enum.Parse<LocationStatus>(source.Status),
            Location = source.Location?.AsSearchModel(),
            CreatedBy = source.CreatedBy,
            ModifiedBy = source.ModifiedBy,
            DateCreated = source.DateCreated,
            DateModified = source.DateModified
        };
    }
}
