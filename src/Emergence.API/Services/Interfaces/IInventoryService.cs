using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;

namespace Emergence.API.Services.Interfaces
{
    public interface IInventoryService
    {
        Task<Inventory> GetInventoryAsync(int id);
        Task<IEnumerable<InventoryItem>> GetInventoryItemsAsync(int inventoryId);
        Task<Inventory> AddOrUpdateInventoryAsync(Inventory inventory);
        Task<InventoryItem> AddOrUpdateInventoryItemAsync(Inventory inventory);
    }
}
