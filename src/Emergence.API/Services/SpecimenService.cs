using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.API.Services.Interfaces;
using Emergence.Data;
using Emergence.Data.Extensions;
using Emergence.Data.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Emergence.API.Services
{
    public class SpecimenService : ISpecimenService
    {
        private readonly IRepository<Data.Shared.Stores.Specimen> _specimenRepository;
        private readonly IInventoryService _inventoryService;

        public SpecimenService(IRepository<Data.Shared.Stores.Specimen> specimenRepository, IInventoryService inventoryService)
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

            if (specimen.InventoryItem.Inventory.InventoryId == 0)
            {
                var inventory = await _inventoryService.AddOrUpdateInventoryAsync(new Data.Shared.Stores.Inventory { UserId = userId }.AsModel());
                specimen.InventoryItem.Inventory.InventoryId = inventory.InventoryId;
            }

            specimen.InventoryItem = await _inventoryService.AddOrUpdateInventoryItemAsync(specimen.InventoryItem);

            var specimenResult = await _specimenRepository.AddOrUpdateAsync(s => s.Id == specimen.SpecimenId, specimen.AsStore());

            return specimenResult.AsModel();
        }

        public async Task<Data.Shared.Models.Specimen> GetSpecimenAsync(long specimenId)
        {
            var result = await _specimenRepository.GetAsync(s => s.Id == specimenId, track: false);
            return result.AsModel();
        }

        public async Task<IEnumerable<Data.Shared.Models.Specimen>> GetSpecimensForInventoryAsync(int inventoryId)
        {
            var specimenResult = _specimenRepository.GetSomeAsync(s => s.InventoryItemId == inventoryId);
            var specimens = new List<Data.Shared.Models.Specimen>();
            await foreach (var specimen in specimenResult)
            {
                specimens.Add(specimen.AsModel());
            }
            return specimens;
        }

        public async Task<IEnumerable<Data.Shared.Models.Specimen>> GetSpecimensByIdsAsync(IEnumerable<int> specimenIds)
        {
            var specimenResult = _specimenRepository.GetSomeAsync(s => specimenIds.Any(i => i == s.Id));
            var specimens = new List<Data.Shared.Models.Specimen>();
            await foreach (var specimen in specimenResult)
            {
                specimens.Add(specimen.AsModel());
            }
            return specimens;
        }

        public async Task<IEnumerable<Data.Shared.Models.Specimen>> FindSpecimens(string search, string userId, int skip = 0, int take = 10)
        {
            var specimenResult = _specimenRepository.GetSomeWithIncludesAsync(s => (s.InventoryItem.Inventory.UserId == userId) &&
                                                                       (EF.Functions.Like(s.InventoryItem.Name, search) ||
                                                                        EF.Functions.Like(s.Lifeform.CommonName, search) ||
                                                                        EF.Functions.Like(s.Lifeform.ScientificName, search)),
                                                                  skip: skip, take: take, track: false,
                                                                  s => s.Include(s => s.InventoryItem).ThenInclude(ii => ii.Inventory).Include(s => s.Lifeform));

            var specimens = new List<Data.Shared.Models.Specimen>();
            await foreach (var specimen in specimenResult)
            {
                specimens.Add(specimen.AsModel());
            }
            return specimens;
        }
    }
}
