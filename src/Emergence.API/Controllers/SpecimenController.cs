using System.Linq;
using System.Threading.Tasks;
using Emergence.Data.Shared;
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
        private readonly IPhotoService _photoService;
        private readonly IOriginService _originService;

        public SpecimenController(ISpecimenService specimenService, IInventoryService inventoryService, IPhotoService photoService, IOriginService originService)
        {
            _specimenService = specimenService;
            _inventoryService = inventoryService;
            _photoService = photoService;
            _originService = originService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Specimen> Get(int id)
        {
            var specimen = await _specimenService.GetSpecimenAsync(id);
            var photos = await _photoService.GetPhotosAsync(PhotoType.Specimen, id);

            specimen.Photos = photos;

            return specimen;
        }

        [HttpPut]

        public async Task<Specimen> Put(Specimen specimen)
        {
            if (specimen.InventoryItem == null)
            {
                specimen.InventoryItem = new InventoryItem();
            }

            if (specimen.InventoryItem.Origin != null && specimen.InventoryItem.Origin.OriginId == 0 && !string.IsNullOrEmpty(specimen.InventoryItem.Origin.Name))
            {
                specimen.InventoryItem.Origin = await _originService.AddOrUpdateOriginAsync(specimen.InventoryItem.Origin, UserId);
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

            specimen.InventoryItem = await _inventoryService.AddOrUpdateInventoryItemAsync(specimen.InventoryItem, UserId);

            var specimenResult = await _specimenService.AddOrUpdateAsync(specimen, UserId);

            if (specimen.Photos != null && specimen.Photos.Any())
            {
                foreach (var photo in specimen.Photos)
                {
                    photo.TypeId = specimenResult.SpecimenId;
                }

                specimenResult.Photos = await _photoService.AddOrUpdatePhotosAsync(specimen.Photos);
            }

            return specimenResult;
        }

        [HttpPost]
        [Route("Find")]
        public async Task<FindResult<Specimen>> FindSpecimens(FindParams findParams)
        {
            var result = await _specimenService.FindSpecimens(findParams, UserId);
            return result;
        }
    }
}
