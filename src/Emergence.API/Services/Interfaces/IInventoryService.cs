using System.Threading.Tasks;
using Emergence.Data.Shared.Models;

namespace Emergence.API.Services.Interfaces
{
    public interface IInventoryService
    {
        Task<Inventory> GetInventory(int id);
        Task<InventoryItem> GetInventoryItems(int id);
    }
}
