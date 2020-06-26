using System;
using System.Collections.Generic;
using System.Text;

namespace Emergence.Data.Stores
{
    public class Inventory
    {
        public int InventoryId { get; set; }
        public long UserId { get; set; }
    }

    public class InventoryItem
    {
        public int InventoryId { get; set; }
        public long InventoryItemId { get; set; }
        public long OriginId { get; set; }
        public string ItemType { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public DateTime? DateAcquired { get; set; }
    }
}
