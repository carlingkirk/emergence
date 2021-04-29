using System;
using System.Text.Json.Serialization;
using Emergence.Data.Shared.Enums;

namespace Emergence.Data.Shared.Models
{
    public class InventoryItem
    {
        public int InventoryItemId { get; set; }
        [JsonConverter(typeof(EnumDisplayConverter<ItemType>))]
        public ItemType ItemType { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        [JsonConverter(typeof(EnumDisplayConverter<ItemStatus>))]
        public ItemStatus Status { get; set; }
        public DateTime? DateAcquired { get; set; }
        [JsonConverter(typeof(EnumDisplayConverter<Visibility>))]
        public Visibility Visibility { get; set; }
        public int? UserId { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public Origin Origin { get; set; }
        public Inventory Inventory { get; set; }
        public UserSummary User { get; set; }
    }
}
