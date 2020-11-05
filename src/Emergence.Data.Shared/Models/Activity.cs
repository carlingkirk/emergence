using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Emergence.Data.Shared.Models
{
    public class Activity : IValidatableObject
    {
        public int ActivityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ActivityType ActivityType { get; set; }
        public string CustomActivityType { get; set; }
        public int? Quantity { get; set; }

        public string AssignedTo { get; set; }
        public DateTime? DateOccurred { get; set; }
        public DateTime? DateScheduled { get; set; }
        public Visibility Visibility { get; set; }
        public int? UserId { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public Specimen Specimen { get; set; }
        public IEnumerable<Photo> Photos { get; set; }
        public UserSummary User { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ActivityType == ActivityType.Unknown)
            {
                yield return new ValidationResult(
                    $"Please choose an activity type.",
                    new[] { nameof(ActivityType) });
            }

            if (ActivityType == ActivityType.Custom && string.IsNullOrEmpty(CustomActivityType))
            {
                yield return new ValidationResult(
                    $"Please enter the custom activity type or change the activity type.",
                    new[] { nameof(CustomActivityType) });
            }

            // Use custom validatory for Name so that all validations are checked at the same time
            if (string.IsNullOrEmpty(Name))
            {
                yield return new ValidationResult(
                    $"Please enter a value for activity title.",
                    new[] { nameof(Name) });
            }

            if (!DateOccurred.HasValue && !DateScheduled.HasValue)
            {
                yield return new ValidationResult(
                    $"Please enter a value for either Date Occurred or Date Scheduled.",
                    new[] { nameof(DateOccurred) });
            }
        }
    }
}
