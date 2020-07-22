using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Extensions;
using Emergence.Data.Shared.Extensions;
using Emergence.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Emergence.Service
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

            if (specimen.InventoryItem.Inventory == null || specimen.InventoryItem.Inventory.InventoryId == 0)
            {
                var inventory = await _inventoryService.GetInventoryAsync(userId);
                if (inventory == null)
                {
                    inventory = await _inventoryService.AddOrUpdateInventoryAsync(new Data.Shared.Stores.Inventory { UserId = userId }.AsModel());
                }
                specimen.InventoryItem.Inventory = inventory;
            }

            specimen.InventoryItem = await _inventoryService.AddOrUpdateInventoryItemAsync(specimen.InventoryItem);

            var specimenResult = await _specimenRepository.AddOrUpdateAsync(s => s.Id == specimen.SpecimenId, specimen.AsStore());

            return specimenResult.AsModel();
        }

        public async Task<Data.Shared.Models.Specimen> GetSpecimenAsync(int specimenId)
        {
            var result = await _specimenRepository.GetWithIncludesAsync(s => s.Id == specimenId, track: false,
                                                                        s => s.Include(s => s.InventoryItem)
                                                                              .Include(ii => ii.InventoryItem.Origin)
                                                                              .Include(s => s.Lifeform));
            return result?.AsModel();
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
            if (search != null)
            {
                search = "%" + search + "%";
            }

            var specimenResult = _specimenRepository.WhereWithIncludesAsync(s => (s.InventoryItem.Inventory.UserId == userId) &&
                                                                       (search == null ||
                                                                       EF.Functions.Like(s.InventoryItem.Name, search) ||
                                                                        EF.Functions.Like(s.Lifeform.CommonName, search) ||
                                                                        EF.Functions.Like(s.Lifeform.ScientificName, search)),
                                                                        s => s.Include(s => s.InventoryItem)
                                                                              .Include(s => s.InventoryItem.Origin)
                                                                              .Include(s => s.Lifeform))
                                                    .WithOrder(s => s.OrderByDescending(s => s.DateCreated))
                                                    .GetSomeAsync(skip: skip, take: take, track: false);

            var specimens = new List<Data.Shared.Models.Specimen>();
            await foreach (var specimen in specimenResult)
            {
                specimens.Add(specimen.AsModel());
            }
            return specimens;
        }
    }
}
