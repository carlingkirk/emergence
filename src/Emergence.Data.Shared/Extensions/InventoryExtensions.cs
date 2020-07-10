using System;
using System.Collections.Generic;
using System.Linq;
using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Extensions
{
    public static class InventoryExtensions
    {
        public static Models.Inventory AsModel(this Inventory source, IEnumerable<InventoryItem> items = null) => new Models.Inventory
        {
            InventoryId = source.Id,
            UserId = source.UserId,
            Items = items != null && items.Any() ? items.Select(i => i.AsModel()) : Enumerable.Empty<Models.InventoryItem>()
        };

        public static Inventory AsStore(this Models.Inventory source) => new Inventory
        {
            Id = source.InventoryId,
            UserId = source.UserId
        };

        public static Models.InventoryItem AsModel(this InventoryItem source) => new Models.InventoryItem
        {
            InventoryItemId = source.Id,
            Inventory = new Models.Inventory { InventoryId = source.InventoryId },
            Name = source.Name,
            Origin = source.OriginId.HasValue ? new Models.Origin { OriginId = source.OriginId.Value } : null,
            ItemType = Enum.Parse<Models.ItemType>(source.ItemType),
            Status = Enum.Parse<Models.Status>(source.Status),
            Quantity = source.Quantity,
            DateAcquired = source.DateAcquired,
            DateCreated = source.DateCreated,
            DateModified = source.DateModified
        };

        public static InventoryItem AsStore(this Models.InventoryItem source) => new InventoryItem
        {
            Id = source.InventoryItemId,
            InventoryId = source.Inventory.InventoryId,
            Name = source.Name,
            OriginId = source.Origin?.OriginId,
            ItemType = source.ItemType.ToString(),
            Status = source.Status.ToString(),
            Quantity = source.Quantity,
            DateAcquired = source.DateAcquired,
            DateCreated = source.DateCreated ?? DateTime.UtcNow,
            DateModified = source.DateModified
        };
    }
}
