using System;
using System.Collections.Generic;

namespace Emergence.Data.Shared.Search.Models
{
    public class Specimen
    {
        public int Id { get; set; }
        public SpecimenStage SpecimenStage { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public IEnumerable<Photo> Photos { get; set; }
        public InventoryItem InventoryItem { get; set; }
        public Lifeform Lifeform { get; set; }
        public Specimen ParentSpecimen { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; }
    }
}
