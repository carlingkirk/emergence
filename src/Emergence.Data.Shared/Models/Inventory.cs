using System;
using System.Collections.Generic;

namespace Emergence.Data.Shared.Models
{
    public class Inventory
    {
        public int InventoryId { get; set; }
        public string OwnerId { get; set; }
        public IEnumerable<InventoryItem> Items { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
