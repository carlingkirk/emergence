using System;
using System.Collections.Generic;
using System.Linq;
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

            if (specimenResult != null)
            {
                var searchSpecimen = await _specimenRepository.GetWithIncludesAsync(s => s.Id == specimenResult.Id, false,
                                                                        s => s.Include(s => s.InventoryItem)
                                                                              .Include(s => s.InventoryItem.Inventory)
                                                                              .Include(s => s.InventoryItem.Origin)
                                                                              .Include(s => s.Lifeform)
                                                                              .Include(s => s.InventoryItem.User)
                                                                              .Include(s => s.InventoryItem.User.Photo));
                await _specimenIndex.IndexAsync(searchSpecimen.AsSearchModel());
            }
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
            if (findParams.Filters == null)
            {
                findParams.Filters = new SpecimenFilters();
            }

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

            if (specimenSearch.Aggregations != null)
            {
                foreach (var aggregation in specimenSearch.AggregationResult)
                {
                    if (aggregation.Name == "Stage")
                    {
                        var filter = findParams.Filters.StageFilter;
                        var values = aggregation.Values;
                        values = values.Prepend(new KeyValuePair<string, long?>("", null)).ToDictionary(k => k.Key, v => v.Value);
                        filter.FacetValues = values;
                    }
                }
            }

            if (specimenSearch.Aggregations != null)
            {
                foreach (var aggregation in specimenSearch.AggregationResult)
                {
                    var filter = SpecimenFindParams.GetFilter(aggregation.Name, findParams);

                    if (filter is SelectFilter<string> selectFilter)
                    {
                        var values = aggregation.Values;
                        values = values.Prepend(new KeyValuePair<string, long?>("", null)).ToDictionary(k => k.Key, v => v.Value);
                        selectFilter.FacetValues = values;
                    }
                    if (filter is SelectRangeFilter<double> selectRangeFilter)
                    {
                        var values = aggregation.Values.ToDictionary(k => double.Parse(k.Key), v => v.Value).OrderBy(k => k.Key).ToDictionary(k => k.Key, v => v.Value);
                        if (aggregation.Name.Contains("Min"))
                        {
                            selectRangeFilter.MinFacetValues = values;
                        }
                        else
                        {
                            selectRangeFilter.MaxFacetValues = values;
                        }
                    }
                    if (filter is RangeFilter<string> rangeFilter)
                    {
                        var values = aggregation.Values;
                        values = values.Prepend(new KeyValuePair<string, long?>("", null)).ToDictionary(k => k.Key, v => v.Value);

                        rangeFilter.FacetValues = values;
                    }
                }
            }

            return new SpecimenFindResult
            {
                Results = specimenIds.Join(specimens, sid => sid, s => s.SpecimenId, (id, s) => s).ToList(),
                Count = specimenSearch.Count,
                Filters = findParams.Filters
            };
        }

        public async Task RemoveSpecimenAsync(Data.Shared.Models.Specimen specimen)
        {
            var result = await _specimenRepository.RemoveAsync(specimen.AsStore());
            if (result)
            {
                var indexResult = await _specimenIndex.RemoveAsync(specimen.SpecimenId.ToString());

                if (!indexResult)
                {
                    Console.WriteLine($"Unable to remove document for specimen Id: {specimen.SpecimenId}");
                }
            }
        }
    }
}
