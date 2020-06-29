using Emergence.Data.Shared.Interfaces;

namespace Emergence.Data.Shared.Models
{
    public class Specimen
    {
        public long SpecimenId { get; set; }
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
