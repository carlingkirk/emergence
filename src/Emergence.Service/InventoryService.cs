using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Shared.Extensions;
using Emergence.Service.Interfaces;

namespace Emergence.Service
{
    public class InventoryService : IInventoryService
    {
        private readonly IRepository<Data.Shared.Stores.Inventory> _inventoryRepository;
        private readonly IRepository<Data.Shared.Stores.InventoryItem> _inventoryItemRepository;
        private readonly IOriginService _originService;

        public InventoryService(IRepository<Data.Shared.Stores.Inventory> inventoryRepository, IRepository<Data.Shared.Stores.InventoryItem> inventoryItemRepository, IOriginService originService)
        {
            _inventoryRepository = inventoryRepository;
            _inventoryItemRepository = inventoryItemRepository;
            _originService = originService;
        }

        public async Task<Data.Shared.Models.Inventory> GetInventoryAsync(int id)
        {
            var result = await _inventoryRepository.GetAsync(i => i.Id == id);
            var inventory = result?.AsModel();
            if (inventory != null)
            {
                inventory.Items = await GetInventoryItemsAsync(id);
            }
            return inventory;
        }

        public async Task<Data.Shared.Models.Inventory> GetInventoryAsync(string userId, bool withItems = false)
        {
            var result = await _inventoryRepository.GetAsync(i => i.OwnerId == userId);
            var inventory = result?.AsModel();
            if (inventory != null && withItems)
            {
                inventory.Items = await GetInventoryItemsAsync(inventory.InventoryId);
            }
            return inventory;
        }

        public async Task<IEnumerable<Data.Shared.Models.InventoryItem>> GetInventoryItemsAsync(int inventoryId)
        {
            var itemResult = _inventoryItemRepository.GetSomeAsync(i => i.InventoryId == inventoryId);
            var items = new List<Data.Shared.Models.InventoryItem>();
            await foreach (var item in itemResult)
            {
                items.Add(item.AsModel());
            }
            return items;
        }

        public async Task<IEnumerable<Data.Shared.Models.InventoryItem>> GetInventoryItemsByIdsAsync(IEnumerable<int> inventoryIds)
        {
            var itemResult = _inventoryItemRepository.GetSomeAsync(i => inventoryIds.Any(id => id == i.Id));
            var items = new List<Data.Shared.Models.InventoryItem>();
            await foreach (var item in itemResult)
            {
                items.Add(item.AsModel());
            }
            return items;
        }

        public async Task<Data.Shared.Models.Inventory> AddOrUpdateInventoryAsync(Data.Shared.Models.Inventory inventory)
        {
            var result = await _inventoryRepository.AddOrUpdateAsync(i => i.Id == inventory.InventoryId, inventory.AsStore());
            return result.AsModel();
        }

        public async Task<Data.Shared.Models.InventoryItem> AddOrUpdateInventoryItemAsync(Data.Shared.Models.InventoryItem inventoryItem, string userId)
        {
            inventoryItem.DateModified = DateTime.UtcNow;

            if (inventoryItem.Origin != null && inventoryItem.Origin.OriginId == 0 && !string.IsNullOrEmpty(inventoryItem.Origin.Name))
            {
                inventoryItem.Origin = await _originService.AddOrUpdateOriginAsync(inventoryItem.Origin, userId);
            }

            var result = await _inventoryItemRepository.AddOrUpdateAsync(i => i.Id == inventoryItem.InventoryItemId, inventoryItem.AsStore());
            return result.AsModel();
        }
    }
}
