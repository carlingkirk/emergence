using System.Threading.Tasks;
using Emergence.API.Services.Interfaces;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Emergence.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        public async Task<Inventory> Get(int id)
        {
            return await _inventoryService.GetInventoryAsync(id);
        }

        [HttpPut]

        public async Task<Inventory> Put(Inventory inventory)
        {
            return await _inventoryService.AddOrUpdateAsync(inventory);
        }
    }
}
