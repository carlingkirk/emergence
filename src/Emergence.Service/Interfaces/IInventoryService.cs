using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;

namespace Emergence.Service.Interfaces
{
    public interface IInventoryService
    {
        Task<Inventory> GetInventoryAsync(int id);
        Task<IEnumerable<InventoryItem>> GetInventoryItemsAsync(int inventoryId);
        Task<IEnumerable<InventoryItem>> GetInventoryItemsByIdsAsync(IEnumerable<int> inventoryIds);
        Task<Inventory> AddOrUpdateInventoryAsync(Inventory inventory);
        Task<InventoryItem> AddOrUpdateInventoryItemAsync(InventoryItem inventory);
    }
}
