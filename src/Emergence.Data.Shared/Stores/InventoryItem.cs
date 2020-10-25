using System;
using System.ComponentModel.DataAnnotations;

namespace Emergence.Data.Shared.Stores
{
    public class InventoryItem : IIncludable<InventoryItem, Inventory>, IAuditable
    {
        public int InventoryId { get; set; }
        public int Id { get; set; }
        public int? OriginId { get; set; }
        [StringLength(36)]
        public string ItemType { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public int Quantity { get; set; }
        [StringLength(36)]
        public string Status { get; set; }
        public DateTime? DateAcquired { get; set; }
        public Visibility Visibility { get; set; }
        [StringLength(36)]
        public string CreatedBy { get; set; }
        [StringLength(36)]
        public string ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public Inventory Inventory { get; set; }
        public Origin Origin { get; set; }
    }
}
