using Emergence.Data;
using Emergence.Data.Shared.Stores;
using NetTopologySuite.Noding;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Emergence.API.Services
{
    public class InventoryService
    {
        private IRepository<Inventory> _inventoryRepository;
        private IRepository<InventoryItem> _inventoryItemRepository;
        public InventoryService(IRepository<Inventory> inventoryRepository, IRepository<InventoryItem> inventoryItemRepository)
        {
            _inventoryRepository = inventoryRepository;
            _inventoryItemRepository = inventoryItemRepository;
        }

        public async Task<Data.Shared.Models.Inventory> GetInventoryAsync(int id)
        {
            var result = await _inventoryRepository.GetAsync(id);
            var items = await GetInventoryItemsAsync(id);
            var inventory = new Data.Shared.Models.Inventory
            {
                InventoryId = result.Id,
                UserId = result.UserId,
                Items = items
            };

            return inventory;
        }

        public async Task<IEnumerable<Data.Shared.Models.InventoryItem>> GetInventoryItemsAsync(int inventoryId)
        {
            var itemResult = _inventoryItemRepository.GetSomeAsync(i => i.InventoryId == inventoryId);
            var items = new List<Data.Shared.Models.InventoryItem>();
            await foreach(var item in itemResult)
            {
                items.Add(new Data.Shared.Models.InventoryItem
                {
                    InventoryItemId = item.Id,
                    InventoryId = item.InventoryId,
                    Name = item.Name,
                    Origin = new Data.Shared.Models.Origin { OriginId = item.OriginId },
                    ItemType = Enum.Parse<Data.Shared.Models.ItemType>(item.ItemType),
                    Status = Enum.Parse<Data.Shared.Models.Status>(item.Status),
                    Quantity = item.Quantity,
                    DateAcquired = item.DateAcquired,
                    DateCreated = item.DateCreated,
                    DateModified = item.DateModified
                });
            }
            return items;
        }
    }
}
