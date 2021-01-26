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
            OwnerId = source.OwnerId,
            Items = items != null && items.Any() ? items.Select(i => i.AsModel()) : Enumerable.Empty<Models.InventoryItem>(),
            CreatedBy = source.CreatedBy,
            ModifiedBy = source.ModifiedBy,
            DateCreated = source.DateCreated,
            DateModified = source.DateModified
        };

        public static Inventory AsStore(this Models.Inventory source) => new Inventory
        {
            Id = source.InventoryId,
            OwnerId = source.OwnerId,
            CreatedBy = source.CreatedBy,
            ModifiedBy = source.ModifiedBy,
            DateCreated = source.DateCreated ?? DateTime.UtcNow,
            DateModified = source.DateModified
        };

        public static Search.Models.Inventory AsSearchModel(this Inventory source) =>
            new Search.Models.Inventory
            {
                Id = source.Id,
                OwnerId = source.OwnerId,
                CreatedBy = source.CreatedBy,
                ModifiedBy = source.ModifiedBy,
                DateCreated = source.DateCreated,
                DateModified = source.DateModified
            };
    }
}
