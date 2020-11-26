using System;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;
using Emergence.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Emergence.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : BaseApiController
    {
        private readonly IInventoryService _inventoryService;
        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Inventory> Get(int id) => await _inventoryService.GetInventoryAsync(id);

        [HttpPut]
        public async Task<Inventory> Put(Inventory inventory)
        {
            if (inventory.OwnerId != UserId)
            {
                throw new UnauthorizedAccessException();
            }

            inventory.OwnerId ??= UserId;
            inventory.CreatedBy ??= UserId;
            inventory.ModifiedBy = UserId;
            inventory.DateCreated ??= DateTime.UtcNow;
            inventory.DateModified = DateTime.UtcNow;

            return await _inventoryService.AddOrUpdateInventoryAsync(inventory);
        }
    }
}
