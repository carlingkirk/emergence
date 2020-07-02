using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.API.Services.Interfaces;
using Emergence.Data;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;

namespace Emergence.API.Services
{
    public class SpecimenService : ISpecimenService
    {
        private readonly IRepository<Specimen> _specimenRepository;
        private readonly IInventoryService _inventoryService;

        public SpecimenService(IRepository<Specimen> specimenRepository, IInventoryService inventoryService)
        {
            _specimenRepository = specimenRepository;
            _inventoryService = inventoryService;
        }

        public async Task<Data.Shared.Models.Specimen> AddOrUpdateAsync(Data.Shared.Models.Specimen specimen, string userId)
        {
            if (specimen.InventoryItem == null)
            {
                specimen.InventoryItem = new Data.Shared.Models.InventoryItem();
            }

            if (specimen.InventoryItem.InventoryId == 0)
            {
                var inventory = await _inventoryService.AddOrUpdateInventoryAsync(new Inventory { UserId = userId }.AsModel());
            }

            specimen.InventoryItem = await _inventoryService.AddOrUpdateInventoryItemAsync(specimen.InventoryItem);

            var specimenResult = await _specimenRepository.AddOrUpdateAsync(s => s.Id == specimen.SpecimenId, specimen.AsStore());

            return specimenResult.AsModel();
        }

        public async Task<Data.Shared.Models.Specimen> GetSpecimenAsync(long specimenId)
        {
            var result = await _specimenRepository.GetAsync(s => s.Id == specimenId);
            return result.AsModel();
        }

        public async Task<IEnumerable<Data.Shared.Models.Specimen>> GetSpecimensAsync(int inventoryId)
        {
            var specimenResult = _specimenRepository.GetSomeAsync(s => s.InventoryItemId == inventoryId);
            var specimens = new List<Data.Shared.Models.Specimen>();
            await foreach (var specimen in specimenResult)
            {
                specimens.Add(specimen.AsModel());
            }
            return specimens;
        }
    }
}
