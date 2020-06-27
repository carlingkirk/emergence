using Emergence.Data;
using Emergence.Data.Shared.Stores;
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

        public async Task<Inventory> GetInventory(int id)
        {
            return await _inventoryRepository.Get(id);
        }

        public async Task<InventoryItem> GetInventoryItems(int id)
        {
            var inventory = await _inventoryRepository.Get(id);
            var items = await _inventoryItemRepository.Get(i => i.InventoryId == inventory.Id);
            return items;
        }
    }
}
