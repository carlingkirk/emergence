using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Emergence.Data.Shared.Models
{
    public class Inventory
    {
        public int InventoryId { get; set; }
        public string UserId { get; set; }
        public IEnumerable<InventoryItem> Items { get; set; }
    }

    public class InventoryItem
    {
        public int InventoryItemId { get; set; }
        public Inventory Inventory { get; set; }
        public ItemType ItemType { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public Status Status { get; set; }
        public Origin Origin { get; set; }
        public DateTime? DateAcquired { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }

    public enum ItemType
    {
        Specimen,
        Supply,
        Container,
        Tool
    }

    public enum Status
    {
        Available,
        Wishlist,
        Ordered,
        [Description("In Use")]
        InUse
    }
}
