using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Extensions;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Search;
using Emergence.Data.Shared.Stores;
using Emergence.Service.Interfaces;
using Emergence.Service.Search;
using Microsoft.EntityFrameworkCore;
using SearchModels = Emergence.Data.Shared.Search.Models;

namespace Emergence.Service
{
    public class SpecimenService : ISpecimenService
    {
        private readonly IRepository<Specimen> _specimenRepository;
        private readonly IIndex<SearchModels.Specimen, Data.Shared.Models.Specimen> _specimenIndex;

        public SpecimenService(IRepository<Specimen> specimenRepository, IIndex<SearchModels.Specimen, Data.Shared.Models.Specimen> specimenIndex)
        {
            _specimenRepository = specimenRepository;
            _specimenIndex = specimenIndex;
        }

        public async Task<Data.Shared.Models.Specimen> AddOrUpdateAsync(Data.Shared.Models.Specimen specimen, string userId)
        {
            var specimenResult = await _specimenRepository.AddOrUpdateAsync(s => s.Id == specimen.SpecimenId, specimen.AsStore());

            return specimenResult.AsModel();
        }

        public async Task<Data.Shared.Models.Specimen> GetSpecimenAsync(int specimenId, Data.Shared.Models.User user)
        {
            var specimenQuery = _specimenRepository.WhereWithIncludes(s => s.Id == specimenId, false,
                                                                      s => s.Include(s => s.InventoryItem)
                                                                              .Include(s => s.InventoryItem.Inventory)
                                                                              .Include(s => s.InventoryItem.Origin)
                                                                              .Include(s => s.Lifeform)
                                                                              .Include(s => s.InventoryItem.User)
                                                                              .Include(s => s.InventoryItem.User.Photo));
            specimenQuery = specimenQuery.CanViewContent(user);

            var specimen = await specimenQuery.FirstOrDefaultAsync();

            return specimen?.AsModel();
        }

        public async Task<SpecimenFindResult> FindSpecimens(SpecimenFindParams findParams, Data.Shared.Models.User user)
        {
            var specimenSearch = await _specimenIndex.SearchAsync(findParams, user);
            var specimenIds = specimenSearch.Documents.Select(p => p.Id).ToArray();
            var specimenQuery = _specimenRepository.WhereWithIncludes(s => specimenIds.Contains(s.Id),
                                                                           false,
                                                                           s => s.Include(s => s.InventoryItem)
                                                                                 .Include(s => s.InventoryItem.Inventory)
                                                                                 .Include(s => s.InventoryItem.Origin)
                                                                                 .Include(s => s.InventoryItem.User)
                                                                                 .Include(s => s.Lifeform));

            specimenQuery = specimenQuery.CanViewContent(user);

            var specimenResult = specimenQuery.GetSomeAsync(track: false);

            var specimens = new List<Data.Shared.Models.Specimen>();
            await foreach (var specimen in specimenResult)
            {
                specimens.Add(specimen.AsModel());
            }

            var filters = new SpecimenFilters();
            if (specimenSearch.Aggregations != null)
            {
                foreach (var aggregation in specimenSearch.Aggregations)
                {
                    if (aggregation.Name == "Stage")
                    {
                        var filter = filters.StageFilter;
                        var values = aggregation.Values;
                        values = values.Prepend(new KeyValuePair<string, long?>("", null)).ToDictionary(k => k.Key, v => v.Value);
                        filter.FacetValues = values;
                    }
                }
            }

            return new SpecimenFindResult
            {
                Results = specimens,
                Count = specimenSearch.Count,
                Filters = filters
            };
        }

        public async Task RemoveSpecimenAsync(Data.Shared.Models.Specimen specimen) => await _specimenRepository.RemoveAsync(specimen.AsStore());

        private IQueryable<Specimen> OrderBy(IQueryable<Specimen> specimenQuery, string sortBy = null, SortDirection sortDirection = SortDirection.None)
        {
            if (sortDirection == SortDirection.None)
            {
                return specimenQuery;
            }

            if (sortBy == null)
            {
                sortBy = "DateCreated";
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
