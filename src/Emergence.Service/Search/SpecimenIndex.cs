using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Search.Models;
using Nest;

namespace Emergence.Service.Search
{
    public class SpecimenIndex : IIndex<Specimen, Data.Shared.Models.Specimen>
    {
        private readonly ISearchClient<Specimen> _searchClient;
        public string IndexName => "specimens_01";
        public string Alias => "specimens";
        public string NameTokenizer => "name_tokenizer";
        public string NameAnalyzer => "name_analyzer";

        public SpecimenIndex(ISearchClient<Specimen> searchClient)
        {
            _searchClient = searchClient;
            _searchClient.ConfigureClient(IndexName, Alias, GetClrMapping, GetMapping, GetSetting);
        }

        public async Task<bool> IndexAsync(Specimen document) => await _searchClient.IndexAsync(document);

        public async Task<BulkIndexResponse> IndexManyAsync(IEnumerable<Specimen> documents) => await _searchClient.IndexManyAsync(documents);

        public async Task<SearchResponse<Specimen>> SearchAsync(FindParams<Data.Shared.Models.Specimen> findParams, Data.Shared.Models.User user)
        {
            var specimenFindParams = findParams as SpecimenFindParams;
            var searchTerm = findParams.SearchText;
            var musts = GetFilters(findParams);
            var shoulds = new List<QueryContainer>();
            var query = new QueryContainerDescriptor<Specimen>();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var fields = new FieldsDescriptor<Specimen>();

                fields = fields.Field(f => f.Name)
                        .Field(f => f.Lifeform.CommonName)
                        .Field(f => f.Lifeform.ScientificName);

                if (findParams.UseNGrams)
                {
                    fields = fields.Field("name.nameSearch");
                }

                shoulds.Add(query.MultiMatch(mm => mm.Fields(mmf => fields)
                            .Query(searchTerm)
                            .Fuzziness(Fuzziness.AutoLength(1, 5))));
            }

            musts.Add(FilterByVisibility(query, user));

            var searchDescriptor = new SearchDescriptor<Specimen>()
                .Query(q => q
                    .Bool(b => b
                        .Should(shoulds.ToArray())
                        .Must(musts.ToArray()).MinimumShouldMatch(string.IsNullOrEmpty(searchTerm) ? 0 : 1)));

            var countDescriptor = new CountDescriptor<Specimen>()
                .Query(q => q
                    .Bool(b => b
                        .Should(shoulds.ToArray())
                        .Must(musts.ToArray()).MinimumShouldMatch(string.IsNullOrEmpty(searchTerm) ? 0 : 1)));

            // Sort
            if (findParams.SortDirection != SortDirection.None)
            {
                if (findParams.SortBy == null)
                {
                    findParams.SortBy = "DateCreated";
                }

                var inventorySorts = GetInvetorySorts();

                if (findParams.SortDirection == SortDirection.Ascending)
                {
                    searchDescriptor.Sort(s => s.Field(f => f.Field(inventorySorts[findParams.SortBy]).Ascending()));
                }
                else
                {
                    searchDescriptor.Sort(s => s.Field(f => f.Field(inventorySorts[findParams.SortBy]).Descending()));
                }
            }

            var aggregations = new AggregationContainerDescriptor<Specimen>();
            var searchFilters = new List<SearchFilter<Specimen>>
            {
                new SearchValueFilter<Specimen, string>("Stage", "specimenStage", specimenFindParams.Filters.StageFilter.Value)
            };

            foreach (var filter in searchFilters)
            {
                if (filter is NestedSearchValueFilter<Specimen, string> nestedFilter)
                {
                    aggregations = nestedFilter.ToAggregationContainerDescriptor(aggregations);
                }
                else if (filter is SearchRangeFilter<Specimen, double> searchRangeFilter)
                {
                    aggregations = searchRangeFilter.ToAggregationContainerDescriptor(aggregations);
                }
                else if (filter is SearchValueFilter<Specimen, string> searchValueFilter)
                {
                    aggregations = searchValueFilter.ToAggregationContainerDescriptor(aggregations);
                }
            }

            searchDescriptor.Aggregations(a => aggregations);

            var response = await _searchClient.SearchAsync(pi => searchDescriptor.Skip(findParams.Skip).Take(findParams.Take), pi => countDescriptor);

            response.AggregationResult = ProcessAggregations(response, specimenFindParams);

            return response;
        }

        public async Task<bool> RemoveAsync(string id) => await _searchClient.RemoveAsync(id);

        private IEnumerable<AggregationResult<Specimen>> ProcessAggregations(SearchResponse<Specimen> response, SpecimenFindParams specimenFindParams)
        {
            var aggregations = new List<AggregationResult<Specimen>>();

            foreach (var aggregation in response.Aggregations)
            {
                var bucketAggregations = new List<AggregationResult<Specimen>>();
                if (aggregation.Value is SingleBucketAggregate singleBucket)
                {
                    foreach (var bucket in singleBucket)
                    {
                        if (bucket.Value is BucketAggregate bucketValues)
                        {
                            var bucketResults = new Dictionary<string, long?>();
                            // Process values
                            foreach (var bucketValue in bucketValues.Items)
                            {
                                var keyedBucket = bucketValue as KeyedBucket<object>;
                                bucketResults.Add(keyedBucket.Key.ToString(), keyedBucket.DocCount);
                            }

                            if (bucketResults.Any())
                            {
                                bucketAggregations.Add(new AggregationResult<Specimen>
                                {
                                    Name = aggregation.Key,
                                    Values = bucketResults
                                });
                            }
                        }
                    }
                    if (!bucketAggregations.Any() && singleBucket.DocCount > 0)
                    {
                        bucketAggregations.Add(new AggregationResult<Specimen>
                        {
                            Name = aggregation.Key,
                            Values = new Dictionary<string, long?> { { specimenFindParams.Filters.StageFilter.Value, singleBucket.DocCount } }
                        });
                    }

                    aggregations.AddRange(bucketAggregations);
                }
                else if (aggregation.Value is BucketAggregate bucket)
                {
                    var bucketResults = new Dictionary<string, long?>();
                    // Process values
                    foreach (var bucketValue in bucket.Items)
                    {
                        var keyedBucket = bucketValue as KeyedBucket<object>;
                        bucketResults.Add(keyedBucket.Key.ToString(), keyedBucket.DocCount);
                    }

                    bucketAggregations.Add(new AggregationResult<Specimen>
                    {
                        Name = aggregation.Key,
                        Values = bucketResults
                    });

                    aggregations.AddRange(bucketAggregations);
                }
            }

            return aggregations;
        }

        private Dictionary<string, Expression<Func<Specimen, object>>> GetInvetorySorts() => new Dictionary<string, Expression<Func<Specimen, object>>>
        {
            { "ScientificName", s => s.Lifeform.ScientificName.Suffix("keyword") },
            { "CommonName", s => s.Lifeform.CommonName.Suffix("keyword") },
            { "Quantity", s => s.Quantity},
            { "Stage", s => s.SpecimenStage },
            { "Status", s => s.InventoryItem.Status },
            { "DateAcquired", s => s.InventoryItem.DateAcquired },
            { "Origin", s => s.InventoryItem.Origin },
            { "DateCreated", s => s.DateCreated }
        };

        private QueryContainer FilterByVisibility(QueryContainerDescriptor<Specimen> query, Data.Shared.Models.User user) =>
            query.Bool(b => b
                    .Should(s => !s.Exists(t => t.Field(f => f.InventoryItem.User)) ||
                                 s.Term(t => t.InventoryItem.Visibility, Visibility.Public) ||
                                 s.Term(t => t.InventoryItem.User.Id, user.Id) ||
                                // Not hidden
                                (!(s.Term(t => t.InventoryItem.Visibility, Visibility.Hidden) ||
                                    (s.Term(t => t.InventoryItem.Visibility, Visibility.Inherit) &&
                                    s.Term(t => t.InventoryItem.User.PlantInfoVisibility, Visibility.Hidden)) ||
                                    (s.Term(t => t.InventoryItem.User.PlantInfoVisibility, Visibility.Inherit) &&
                                    s.Term(t => t.InventoryItem.User.ProfileVisibility, Visibility.Hidden))) &&
                                    // Inherited
                                    ((s.Term(t => t.InventoryItem.Visibility, Visibility.Inherit) &&
                                        (s.Term(t => t.InventoryItem.User.PlantInfoVisibility, Visibility.Public) ||
                                        (s.Term(t => t.InventoryItem.User.PlantInfoVisibility, Visibility.Inherit) &&
                                        s.Term(t => t.InventoryItem.User.ProfileVisibility, Visibility.Public)) ||
                                        (s.Term(t => t.InventoryItem.User.PlantInfoVisibility, Visibility.Contacts) &&
                                        s.Term(t => t.InventoryItem.User.ContactIds, user.Id)))) ||
                                    // Contacts
                                    (s.Term(t => t.InventoryItem.Visibility, Visibility.Contacts) &&
                                    s.Term(t => t.InventoryItem.User.ContactIds, user.Id))))));

        private List<QueryContainer> GetFilters(FindParams<Data.Shared.Models.Specimen> findParams)
        {
            var musts = new List<QueryContainer>();
            var query = new QueryContainerDescriptor<Specimen>();
            var specimenFindParams = findParams as SpecimenFindParams;
            if (findParams.CreatedBy != null)
            {
                musts.Add(query.Match(m => m.Field(f => f.CreatedBy).Query(findParams.CreatedBy)));
            }

            if (specimenFindParams.Filters != null)
            {
                var filters = specimenFindParams.Filters;

                var stageFilter = filters.StageFilter;

                if (!string.IsNullOrEmpty(stageFilter.Value))
                {
                    musts.Add(query.Term(t => t.SpecimenStage, stageFilter.Value));
                }
            }

            return musts;
        }

        private ITypeMapping GetMapping(TypeMappingDescriptor<Specimen> mapping) =>
            mapping.AutoMap()
            .Properties(pi => pi
            .Text(t => t.Name(n => n.Name).Fields(f => f.Text(t => t.Name("nameSearch").Analyzer(NameAnalyzer)))));

        private IPromise<IndexSettings> GetSetting(IndexSettingsDescriptor setting) =>
            setting.Analysis(a => a
                .Analyzers(azs => azs.Custom(NameAnalyzer, c => c.Tokenizer(NameTokenizer).Filters("lowercase")))
                .Tokenizers(t => t.EdgeNGram(NameTokenizer, ng => ng.MinGram(3).MaxGram(20).TokenChars(new[] { TokenChar.Letter }))));

        private IClrTypeMapping<Specimen> GetClrMapping(ClrTypeMappingDescriptor<Specimen> mapping) =>
            mapping.IndexName(IndexName)
                .PropertyName(s => s.Id, "id")
                .PropertyName(s => s.Name, "name")
                .PropertyName(s => s.Quantity, "quantity")
                .PropertyName(s => s.InventoryItem, "inventoryItem")
                .PropertyName(s => s.CreatedBy, "createdBy")
                .PropertyName(s => s.ModifiedBy, "modifiedBy")
                .PropertyName(s => s.DateCreated, "dateCreated")
                .PropertyName(s => s.DateModified, "dateModified");
    }
}
