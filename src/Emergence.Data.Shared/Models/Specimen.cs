using System;
using System.Collections.Generic;

namespace Emergence.Data.Shared.Models
{
    public class Specimen
    {
        public int SpecimenId { get; set; }
        public SpecimenStage SpecimenStage { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public InventoryItem InventoryItem { get; set; }
        public IEnumerable<Photo> Photos { get; set; }
        public Lifeform Lifeform { get; set; }
        public PlantInfo PlantInfo { get; set; }
    }
}
