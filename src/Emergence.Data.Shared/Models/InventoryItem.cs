using System;

namespace Emergence.Data.Shared.Models
{
    public class InventoryItem
    {
        public int InventoryItemId { get; set; }
        public ItemType ItemType { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public ItemStatus Status { get; set; }
        public DateTime? DateAcquired { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public Origin Origin { get; set; }
        public Inventory Inventory { get; set; }
    }
}
