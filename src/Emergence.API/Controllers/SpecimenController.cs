using System;
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
        private readonly IUserService _userService;

        public SpecimenController(ISpecimenService specimenService, IInventoryService inventoryService, IPhotoService photoService, IOriginService originService, IUserService userService)
        {
            _specimenService = specimenService;
            _inventoryService = inventoryService;
            _photoService = photoService;
            _originService = originService;
            _userService = userService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Specimen> Get(int id)
        {
            var user = await _userService.GetIdentifyingUser(UserId);
            var specimen = await _specimenService.GetSpecimenAsync(id, user);

            if (specimen.InventoryItem.User?.PhotoId != null)
            {
                var userPhoto = await _photoService.GetPhotoAsync(specimen.InventoryItem.User.PhotoId.Value);
                specimen.InventoryItem.User.PhotoThumbnailUri = userPhoto.ThumbnailUri;
            }

            var photos = await _photoService.GetPhotosAsync(PhotoType.Specimen, id);

            specimen.Photos = photos;

            return specimen;
        }

        [HttpPut]

        public async Task<Specimen> Put(Specimen specimen)
        {
            specimen.CreatedBy = UserId;

            if (specimen.InventoryItem == null)
            {
                specimen.InventoryItem = new InventoryItem { CreatedBy = UserId };
            }

            specimen.InventoryItem.CreatedBy = UserId;
            var userId = await _userService.GetUserIdAsync(UserId);
            specimen.InventoryItem.UserId = userId;

            if (specimen.InventoryItem.Origin != null && specimen.InventoryItem.Origin.OriginId == 0)
            {
                specimen.InventoryItem.Origin.UserId = userId;
                specimen.InventoryItem.Origin = await _originService.AddOrUpdateOriginAsync(specimen.InventoryItem.Origin, UserId);
            }

            if (specimen.InventoryItem.Inventory == null || specimen.InventoryItem.Inventory.InventoryId == 0)
            {
                var inventory = await _inventoryService.GetInventoryAsync(UserId);
                if (inventory == null)
                {
                    inventory = await _inventoryService.AddOrUpdateInventoryAsync(new Inventory { OwnerId = UserId, CreatedBy = UserId, DateCreated = DateTime.Now });
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
            var user = await _userService.GetIdentifyingUser(UserId);
            var result = await _specimenService.FindSpecimens(findParams, user);
            return result;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetIdentifyingUser(UserId);
            var specimen = await _specimenService.GetSpecimenAsync(id, user);
            if (specimen.CreatedBy != UserId)
            {
                return Unauthorized();
            }

            var photos = await _photoService.GetPhotosAsync(PhotoType.Specimen, specimen.SpecimenId);
            await _photoService.RemovePhotosAsync(photos);
            await _specimenService.RemoveSpecimenAsync(specimen);

            return Ok();
        }
    }
}
