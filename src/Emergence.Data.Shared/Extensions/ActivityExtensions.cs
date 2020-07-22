using System;
using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Extensions
{
    public static class ActivityExtensions
    {
        public static Models.Activity AsModel(this Activity source) => new Models.Activity
        {
            ActivityId = source.Id,
            ActivityType = Enum.Parse<Models.ActivityType>(source.ActivityType),
            Name = source.Name,
            Description = source.Description,
            Specimen = source.Specimen != null ? source.Specimen.AsModel() : source.SpecimenId.HasValue ? new Models.Specimen { SpecimenId = source.SpecimenId.Value } : null,
            UserId = source.UserId,
            DateOccured = source.DateOccured,
            DateCreated = source.DateCreated,
            DateModified = source.DateModified
        };

        public static Activity AsStore(this Models.Activity source) => new Activity
        {
            Id = source.ActivityId,
            Name = source.Name,
            Description = source.Description,
            ActivityType = source.ActivityType.ToString(),
            SpecimenId = source.Specimen?.SpecimenId,
            UserId = source.UserId,
            DateOccured = source.DateOccured,
            DateCreated = source.DateCreated ?? DateTime.UtcNow,
            DateModified = source.DateModified
        };
    }
}
