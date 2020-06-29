using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.API.Services.Interfaces;
using Emergence.Data;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;

namespace Emergence.API.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IRepository<Inventory> _inventoryRepository;
        private readonly IRepository<InventoryItem> _inventoryItemRepository;
        public InventoryService(IRepository<Inventory> inventoryRepository, IRepository<InventoryItem> inventoryItemRepository)
        {
            _inventoryRepository = inventoryRepository;
            _inventoryItemRepository = inventoryItemRepository;
        }

        public async Task<Data.Shared.Models.Inventory> GetInventoryAsync(int id)
        {
            var result = await _inventoryRepository.GetAsync(id);
            var inventory = result.AsModel();
            inventory.Items = await GetInventoryItemsAsync(id);
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

        public async Task<Data.Shared.Models.Inventory> AddOrUpdateAsync(Data.Shared.Models.Inventory inventory)
        {
            var result = await _inventoryRepository.AddOrUpdateAsync(inventory.InventoryId, inventory.AsStore());
            return result.AsModel();
        }
    }
}
