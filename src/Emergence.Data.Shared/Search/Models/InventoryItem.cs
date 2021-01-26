using System;

namespace Emergence.Data.Shared.Search.Models
{
    public class InventoryItem
    {
        public int Id { get; set; }
        public ItemType ItemType { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public ItemStatus Status { get; set; }
        public DateTime? DateAcquired { get; set; }
        public Visibility Visibility { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public User User { get; set; }
        public Inventory Inventory { get; set; }
        public Origin Origin { get; set; }
    }

    public class Inventory
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
