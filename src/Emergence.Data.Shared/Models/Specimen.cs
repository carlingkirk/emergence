namespace Emergence.Data.Shared.Models
{
    public class Specimen
    {
        public int SpecimenId { get; set; }
        public Plant Plant { get; set; }
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
