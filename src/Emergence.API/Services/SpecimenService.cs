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
        public SpecimenService(IRepository<Specimen> specimenRepository)
        {
            _specimenRepository = specimenRepository;
        }
        public async Task<Data.Shared.Models.Specimen> AddOrUpdateAsync(Data.Shared.Models.Specimen specimen)
        {
            var result = await _specimenRepository.AddOrUpdateAsync(specimen.AsStore());
            return result.AsModel();
        }

        public async Task<Data.Shared.Models.Specimen> GetSpecimenAsync(long specimenId)
        {
            var result = await _specimenRepository.GetAsync(specimenId);
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
