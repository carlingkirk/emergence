using Emergence.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Emergence.Data.Models
{
    public class Specimen
    {
        public ILifeform Lifeform { get; set; }
        public SpecimenStage SpecimenStage { get; set; }
        public InventoryItem InventoryItem { get; set; }
    }

    public enum SpecimenStage
    {
        Seed,
        Ordered,
        Stratification,
        Germination,
        Growing,
        InGround,
        Blooming,
        Diseased,
        Deceased
    }
}
