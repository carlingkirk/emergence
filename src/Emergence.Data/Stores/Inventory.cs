using Emergence.Data.Database;
using System;

namespace Emergence.Data.Stores
{
    public class Inventory : IKeyable
    {
        public object Key => Id;
        public int Id { get; set; }
        public long UserId { get; set; }
    }

    public class InventoryItem
    {
        public int InventoryId { get; set; }
        public long Id { get; set; }
        public long OriginId { get; set; }
        public string ItemType { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public DateTime? DateAcquired { get; set; }
    }
}
