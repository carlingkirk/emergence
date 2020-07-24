using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;
using Emergence.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Emergence.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecimenController : BaseAPIController
    {
        private readonly ISpecimenService _specimenService;
        private readonly IInventoryService _inventoryService;
        public SpecimenController(ISpecimenService specimenService, IInventoryService inventoryService)
        {
            _specimenService = specimenService;
            _inventoryService = inventoryService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Specimen> Get(int id) => await _specimenService.GetSpecimenAsync(id);

        [HttpPut]

        public async Task<Specimen> Put(Specimen specimen)
        {
            if (specimen.InventoryItem == null)
            {
                specimen.InventoryItem = new InventoryItem();
            }

            if (specimen.InventoryItem.Inventory == null || specimen.InventoryItem.Inventory.InventoryId == 0)
            {
                var inventory = await _inventoryService.GetInventoryAsync(UserId);
                if (inventory == null)
                {
                    inventory = await _inventoryService.AddOrUpdateInventoryAsync(new Inventory { UserId = UserId });
                }
                specimen.InventoryItem.Inventory = inventory;
            }

            specimen.InventoryItem = await _inventoryService.AddOrUpdateInventoryItemAsync(specimen.InventoryItem);

            return await _specimenService.AddOrUpdateAsync(specimen, UserId);
        }

        [HttpGet]
        [Route("Find")]
        public async Task<IEnumerable<Specimen>> FindSpecimens(string search = null, int skip = 0, int take = 10)
        {
            var results = await _specimenService.FindSpecimens(search, UserId, skip, take);
            return results;
        }
    }
}
