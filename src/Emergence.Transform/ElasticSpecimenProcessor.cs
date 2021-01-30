using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Extensions;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;
using Emergence.Service.Search;
using SearchModels = Emergence.Data.Shared.Search.Models;

namespace Emergence.Transform
{
    public class ElasticSpecimenProcessor : IElasticProcessor<Specimen>
    {
        private readonly IRepository<Specimen> _specimenRepository;
        private readonly IIndex<SearchModels.Specimen, Emergence.Data.Shared.Models.Specimen> _specimenIndex;

        public ElasticSpecimenProcessor(IRepository<Specimen> specimenRepository, IIndex<SearchModels.Specimen, Emergence.Data.Shared.Models.Specimen> specimenIndex)
        {
            _specimenIndex = specimenIndex;
            _specimenRepository = specimenRepository;
        }

        public async Task<BulkIndexResponse> Process(int startId, int endId)
        {
            var specimenQuery = _specimenRepository.WhereWithIncludes(s => s.Id >= startId && s.Id < endId, false,
                                                                        s => s.Include(s => s.Lifeform)
                                                                              .Include(s => s.InventoryItem.Inventory)
                                                                              .Include(s => s.InventoryItem.Origin)
                                                                              .Include(s => s.InventoryItem.Origin.Location)
                                                                              .Include(s => s.InventoryItem.User)
                                                                              .Include(s => s.InventoryItem.User.Photo)
                                                                              .Include(s => s.InventoryItem.User.Contacts)
                                                                              .Include(s => s.ParentSpecimen));

            var specimenResult = await specimenQuery.GetAllAsync();
            if (specimenResult.Count() == 0)
            {
                return new BulkIndexResponse { Successes = 0, Failures = 0 };
            }

            var specimens = new List<SearchModels.Specimen>();
            foreach (var specimen in specimenResult.GroupBy(i => i.Id))
            {
                var specimenKey = specimen.First();
                specimens.Add(specimenKey.AsSearchModel());
            }

            return await _specimenIndex.IndexManyAsync(specimens);
        }

        public Task<BulkIndexResponse> ProcessSome() => throw new System.NotImplementedException();
    }
}
