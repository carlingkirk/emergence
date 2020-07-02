using System;
using System.Threading.Tasks;
using Emergence.API.Services.Interfaces;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Emergence.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : BaseAPIController
    {
        private readonly IInventoryService _inventoryService;
        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        public async Task<Inventory> Get(int id) => await _inventoryService.GetInventoryAsync(id);

        [HttpPut]
        public async Task<Inventory> Put(Inventory inventory)
        {
            if (inventory.UserId == null)
            {
                inventory.UserId = UserId;
            }

            if (inventory.UserId != UserId)
            {
                throw new UnauthorizedAccessException();
            }

            return await _inventoryService.AddOrUpdateInventoryAsync(inventory);
        }
    }
}
