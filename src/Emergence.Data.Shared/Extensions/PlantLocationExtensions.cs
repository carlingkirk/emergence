using System;
using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Extensions
{
    public static class PlantLocationExtensions
    {
        public static Models.PlantLocation AsModel(this PlantLocation source) => new Models.PlantLocation
        {
            Id = source.Id,
            Status = !string.IsNullOrEmpty(source.Status) ? Enum.Parse<LocationStatus>(source.Status) : LocationStatus.Unknown,
            ConservationStatus = !string.IsNullOrEmpty(source.ConservationStatus) ? Enum.Parse<ConservationStatus>(source.ConservationStatus) : ConservationStatus.Unknown,
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
            Status = source.Status != LocationStatus.Unknown ? source.Status.ToString() : null,
            ConservationStatus = source.ConservationStatus != ConservationStatus.Unknown ? source.ConservationStatus.ToString() : null,
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
            Status = !string.IsNullOrEmpty(source.Status) ? Enum.Parse<LocationStatus>(source.Status) : LocationStatus.Unknown,
            ConservationStatus = !string.IsNullOrEmpty(source.ConservationStatus) ? Enum.Parse<ConservationStatus>(source.ConservationStatus) : ConservationStatus.Unknown,
            Location = source.Location?.AsSearchModel(),
            CreatedBy = source.CreatedBy,
            ModifiedBy = source.ModifiedBy,
            DateCreated = source.DateCreated,
            DateModified = source.DateModified
        };

        public static Search.Models.PlantLocation AsSearchModel(this Models.PlantLocation source) => new Search.Models.PlantLocation
        {
            Id = source.Id,
            Status = source.Status,
            ConservationStatus = source.ConservationStatus,
            Location = source.Location?.AsSearchModel(),
            CreatedBy = source.CreatedBy,
            ModifiedBy = source.ModifiedBy,
            DateCreated = source.DateCreated ?? DateTime.UtcNow,
            DateModified = source.DateModified
        };
    }
}
