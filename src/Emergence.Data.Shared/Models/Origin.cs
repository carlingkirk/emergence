using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Emergence.Data.Shared.Models
{
    public class Origin : IValidatableObject
    {
        public int OriginId { get; set; }

        public string Name { get; set; }
        public OriginType Type { get; set; }
        public string Description { get; set; }
        public string Authors { get; set; }
        public Uri Uri { get; set; }
        public int? LocationId { get; set; }
        public string ExternalId { get; set; }
        public string AltExternalId { get; set; }
        public Visibility Visibility { get; set; }
        public int? UserId { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public string ShortUri => Uri != null ? Uri.ToString().Length > 40 ? Uri.ToString().Substring(0, 40) + "..." : Uri.ToString() : null;
        public string TinyUri => Uri != null ? Uri.ToString().Length > 20 ? Uri.ToString().Substring(0, 20) + "..." : Uri.ToString() : null;

        public Location Location { get; set; }
        public Origin ParentOrigin { get; set; }
        public UserSummary User { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Name))
            {
                yield return new ValidationResult(
                    $"Please enter a name.",
                    new[] { nameof(Name) });
            }

            if (Type == OriginType.Unknown)
            {
                yield return new ValidationResult(
                    $"Please choose a type.",
                    new[] { nameof(Type) });
            }
        }
    }
}
