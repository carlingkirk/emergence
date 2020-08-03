using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Extensions;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;
using Emergence.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Emergence.Service
{
    public class SpecimenService : ISpecimenService
    {
        private readonly IRepository<Data.Shared.Stores.Specimen> _specimenRepository;

        public SpecimenService(IRepository<Data.Shared.Stores.Specimen> specimenRepository)
        {
            _specimenRepository = specimenRepository;
        }

        public async Task<Data.Shared.Models.Specimen> AddOrUpdateAsync(Data.Shared.Models.Specimen specimen, string userId)
        {
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

        public async Task<FindResult<Data.Shared.Models.Specimen>> FindSpecimens(string search, string userId, int skip = 0, int take = 10, string sortBy = null,
            SortDirection sortDirection = SortDirection.Ascending)
        {
            if (search != null)
            {
                search = "%" + search + "%";
            }

            var specimenQuery = _specimenRepository.WhereWithIncludesAsync(s => (s.InventoryItem.Inventory.UserId == userId) &&
                                                                       (search == null ||
                                                                       EF.Functions.Like(s.InventoryItem.Name, search) ||
                                                                        EF.Functions.Like(s.Lifeform.CommonName, search) ||
                                                                        EF.Functions.Like(s.Lifeform.ScientificName, search)),
                                                                        s => s.Include(s => s.InventoryItem)
                                                                              .Include(s => s.InventoryItem.Origin)
                                                                              .Include(s => s.Lifeform));
            specimenQuery = OrderBy(specimenQuery, sortBy, sortDirection);

            var count = specimenQuery.Count();
            var specimenResult = specimenQuery.GetSomeAsync(skip: skip, take: take, track: false);

            var specimens = new List<Data.Shared.Models.Specimen>();
            await foreach (var specimen in specimenResult)
            {
                specimens.Add(specimen.AsModel());
            }
            return new FindResult<Data.Shared.Models.Specimen>
            {
                Results = specimens,
                Count = count
            };
        }

        private IQueryable<Specimen> OrderBy(IQueryable<Specimen> specimenQuery, string sortBy = "DateCreated",
            SortDirection sortDirection = SortDirection.None)
        {
            if (sortDirection == SortDirection.None)
            {
                return specimenQuery;
            }

            var specimenSorts = new Dictionary<string, Expression<Func<Specimen, object>>>
            {
                { "ScientificName", s => s.Lifeform.ScientificName },
                { "CommonName", s => s.Lifeform.CommonName },
                { "Quantity", s => s.InventoryItem.Quantity },
                { "Stage", s => s.SpecimenStage },
                { "Status", s => s.InventoryItem.Status },
                { "DateAcquired", s => s.InventoryItem.DateAcquired },
                { "Origin", s => s.InventoryItem.Origin },
                { "DateCreated", s => s.DateCreated }
            };

            if (sortDirection == SortDirection.Descending)
            {
                specimenQuery = specimenQuery.WithOrder(p => p.OrderByDescending(specimenSorts[sortBy]));
            }
            else
            {
                specimenQuery = specimenQuery.WithOrder(p => p.OrderBy(specimenSorts[sortBy]));
            }

            return specimenQuery;
        }
    }
}
