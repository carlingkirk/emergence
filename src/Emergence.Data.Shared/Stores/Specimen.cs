namespace Emergence.Data.Shared.Stores
{
    public class Specimen : IIncludable<Specimen>, IIncludable<Specimen, InventoryItem>, IIncludable<Specimen, Lifeform>
    {
        public int Id { get; set; }
        public int InventoryItemId { get; set; }
        public int? LifeformId { get; set; }
        public string SpecimenStage { get; set; }
        public Lifeform Lifeform { get; set; }
        public InventoryItem InventoryItem { get; set; }
    }
}
