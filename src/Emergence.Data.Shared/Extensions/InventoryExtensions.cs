using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emergence.Data.Shared.Models;

namespace Emergence.Data.Shared.Extensions
{
    public static class InventoryExtensions
    {
        public static Data.Shared.Models.Inventory AsModel(this Data.Shared.Stores.Inventory source, IEnumerable<Data.Shared.Stores.InventoryItem> items = null)
        {
            return new Data.Shared.Models.Inventory
            {
                InventoryId = source.Id,
                UserId = source.UserId,
                Items = items.Any() ? items.Select(i => i.AsModel()) : Enumerable.Empty<Data.Shared.Models.InventoryItem>()
            };
        }

        public static Data.Shared.Stores.Inventory AsStore(this Data.Shared.Models.Inventory source)
        {
            return new Data.Shared.Stores.Inventory
            {
                Id = source.InventoryId,
                UserId = source.UserId
            };
        }

        public static Data.Shared.Models.InventoryItem AsModel(this Data.Shared.Stores.InventoryItem source)
        {
            return new Data.Shared.Models.InventoryItem
            {
                InventoryItemId = source.Id,
                InventoryId = source.InventoryId,
                Name = source.Name,
                Origin = source.OriginId.HasValue ? new Data.Shared.Models.Origin { OriginId = source.OriginId.Value } : null,
                ItemType = Enum.Parse<Data.Shared.Models.ItemType>(source.ItemType),
                Status = Enum.Parse<Data.Shared.Models.Status>(source.Status),
                Quantity = source.Quantity,
                DateAcquired = source.DateAcquired,
                DateCreated = source.DateCreated,
                DateModified = source.DateModified
            };
        }

        public static Data.Shared.Stores.InventoryItem AsStore(this Data.Shared.Models.InventoryItem source)
        {
            return new Data.Shared.Stores.InventoryItem
            {
                Id = source.InventoryItemId,
                InventoryId = source.InventoryId,
                Name = source.Name,
                OriginId = source.Origin?.OriginId,
                ItemType = source.ItemType.ToString(),
                Status = source.Status.ToString(),
                Quantity = source.Quantity,
                DateAcquired = source.DateAcquired,
                DateCreated = source.DateCreated,
                DateModified = source.DateModified
            };
        }
    }
}
