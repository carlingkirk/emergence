using System;

namespace Emergence.Data.Shared.Stores
{
    public class Inventory : IAuditable
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }

    public class InventoryItem : IIncludable<InventoryItem, Inventory>, IAuditable
    {
        public int InventoryId { get; set; }
        public int Id { get; set; }
        public int? OriginId { get; set; }
        public string ItemType { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public DateTime? DateAcquired { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public Inventory Inventory { get; set; }
        public Origin Origin { get; set; }
    }
}
