using System;
using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Extensions
{
    public static class ActivityExtensions
    {
        public static Models.Activity AsModel(this Activity source) => new Models.Activity
        {
            ActivityId = source.Id,
            ActivityType = Enum.Parse<ActivityType>(source.ActivityType),
            CustomActivityType = source.CustomActivityType,
            Name = source.Name,
            Description = source.Description,
            Specimen = source.Specimen != null ? source.Specimen.AsModel() : source.SpecimenId.HasValue ? new Models.Specimen { SpecimenId = source.SpecimenId.Value } : null,
            Quantity = source.Quantity,
            AssignedTo = source.AssignedTo,
            DateOccurred = source.DateOccurred,
            DateScheduled = source.DateScheduled,
            Visibility = source.Visibility,
            CreatedBy = source.CreatedBy,
            ModifiedBy = source.ModifiedBy,
            DateCreated = source.DateCreated,
            DateModified = source.DateModified
        };

        public static Activity AsStore(this Models.Activity source) => new Activity
        {
            Id = source.ActivityId,
            Name = source.Name,
            Description = source.Description,
            ActivityType = source.ActivityType.ToString(),
            CustomActivityType = source.CustomActivityType,
            SpecimenId = source.Specimen?.SpecimenId,
            Quantity = source.Quantity,
            AssignedTo = source.AssignedTo,
            DateOccurred = source.DateOccurred,
            DateScheduled = source.DateScheduled,
            Visibility = source.Visibility,
            CreatedBy = source.CreatedBy,
            ModifiedBy = source.ModifiedBy,
            DateCreated = source.DateCreated ?? DateTime.UtcNow,
            DateModified = source.DateModified
        };
    }
}
