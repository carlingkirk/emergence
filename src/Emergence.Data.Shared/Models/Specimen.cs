using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Emergence.Data.Shared.Enums;

namespace Emergence.Data.Shared.Models
{
    public class Specimen : IValidatableObject
    {
        public int SpecimenId { get; set; }
        [JsonConverter(typeof(EnumDisplayConverter<SpecimenStage>))]
        public SpecimenStage SpecimenStage { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public InventoryItem InventoryItem { get; set; }
        public IEnumerable<Photo> Photos { get; set; }
        public Lifeform Lifeform { get; set; }
        public PlantInfo PlantInfo { get; set; }
        public Specimen ParentSpecimen { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Name))
            {
                yield return new ValidationResult(
                    $"Please enter a name.",
                    new[] { nameof(Name) });
            }

            if (Quantity == 0)
            {
                yield return new ValidationResult(
                    $"Please enter a quantity.",
                    new[] { nameof(Quantity) });
            }

            if (SpecimenStage == SpecimenStage.Unknown)
            {
                yield return new ValidationResult(
                    $"Please choose a stage.",
                    new[] { nameof(SpecimenStage) });
            }
        }
    }
}
