namespace Emergence.Data.Shared.Stores
{
    public class Specimen : IKeyable
    {
        public object Key => Id;
        public long Id { get; set; }
        public long InventoryItemId { get; set; }
        public string SpecimenStage { get; set; }
    }
}
