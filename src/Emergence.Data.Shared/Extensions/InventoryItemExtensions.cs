using System;
using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Extensions
{
    public static class InventoryItemExtensions
    {
        public static Models.InventoryItem AsModel(this InventoryItem source) => new Models.InventoryItem
        {
            InventoryItemId = source.Id,
            Inventory = source.Inventory != null ? source.Inventory.AsModel() : source.InventoryId > 0 ? new Models.Inventory { InventoryId = source.InventoryId } : null,
            Name = source.Name,
            Origin = source.Origin != null ? source.Origin.AsModel() : source.OriginId.HasValue ? new Models.Origin { OriginId = source.OriginId.Value } : null,
            ItemType = Enum.Parse<ItemType>(source.ItemType),
            Status = Enum.Parse<ItemStatus>(source.Status),
            Quantity = source.Quantity,
            DateAcquired = source.DateAcquired,
            Visibility = source.Visibility,
            UserId = source.UserId,
            User = source.User != null ? source.User.AsModel() : source.UserId.HasValue ? new Models.User { Id = source.UserId.Value } : null,
            CreatedBy = source.CreatedBy,
            ModifiedBy = source.ModifiedBy,
            DateCreated = source.DateCreated,
            DateModified = source.DateModified
        };

        public static InventoryItem AsStore(this Models.InventoryItem source) => new InventoryItem
        {
            Id = source.InventoryItemId,
            InventoryId = (int)source.Inventory?.InventoryId,
            Name = source.Name,
            OriginId = source.Origin?.OriginId,
            ItemType = source.ItemType.ToString(),
            Status = source.Status.ToString(),
            Quantity = source.Quantity,
            DateAcquired = source.DateAcquired,
            Visibility = source.Visibility,
            UserId = source.UserId,
            CreatedBy = source.CreatedBy,
            ModifiedBy = source.ModifiedBy,
            DateCreated = source.DateCreated ?? DateTime.UtcNow,
            DateModified = source.DateModified
        };
    }
}
