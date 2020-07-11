namespace Emergence.Data.Shared.Stores
{
    public class Specimen
    {
        public int Id { get; set; }
        public int InventoryItemId { get; set; }
        public InventoryItem InventoryItem { get; set; }
        public int? LifeformId { get; set; }
        public Lifeform Lifeform { get; set; }
        public string SpecimenStage { get; set; }
    }
}
