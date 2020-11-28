using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Emergence.Data.Shared.Interfaces;

namespace Emergence.Data.Shared.Models
{
    public class PlantInfo : ILifeform
    {
        public int PlantInfoId { get; set; }
        public int LifeformId { get; set; }
        public string ScientificName { get; set; }
        public string CommonName { get; set; }
        public bool? Preferred { get; set; }
        public BloomTime BloomTime { get; set; }
        public Height Height { get; set; }
        public Spread Spread { get; set; }
        public Requirements Requirements { get; set; }
        public Visibility Visibility { get; set; }
        public int? UserId { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public Taxon Taxon { get; set; }
        public Origin Origin { get; set; }
        public Lifeform Lifeform { get; set; }
        public IEnumerable<PlantLocation> Locations { get; set; }
        public IEnumerable<Photo> Photos { get; set; }
        public UserSummary User { get; set; }

        [Required(ErrorMessage = "Please find a plant by searching in the Plant Name field by scientific name or common name.")]
        public Lifeform SelectedLifeform { get; set; }
    }
}
