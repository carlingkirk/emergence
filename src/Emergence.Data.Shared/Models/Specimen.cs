using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Emergence.Data.Shared.Models
{
    public class Specimen
    {
        public int SpecimenId { get; set; }
        public Lifeform Lifeform { get; set; }
        public PlantInfo PlantInfo { get; set; }
        public SpecimenStage SpecimenStage { get; set; }
        public InventoryItem InventoryItem { get; set; }
        public IEnumerable<Photo> Photos { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }

    public enum SpecimenStage
    {
        Seed,
        Ordered,
        Stratification,
        Germination,
        Growing,
        [Description("In Ground")]
        InGround,
        Blooming,
        Diseased,
        Deceased
    }
}
