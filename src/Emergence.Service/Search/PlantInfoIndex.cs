using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Search;
using Emergence.Data.Shared.Search.Models;
using Nest;

namespace Emergence.Service.Search
{
    public class PlantInfoIndex : IIndex<PlantInfo, Data.Shared.Models.PlantInfo>, IIndex<Lifeform, Data.Shared.Models.Lifeform>
    {
        private readonly ISearchClient<PlantInfo> _searchClient;
        public string IndexName => "plant_infos_02";
        public string Alias => "plant_infos";
        public string NameTokenizer => "name_tokenizer";
        public string NameAnalyzer => "name_analyzer";

        public PlantInfoIndex(ISearchClient<PlantInfo> searchClient)
        {
            _searchClient = searchClient;
            _searchClient.ConfigureClient(IndexName, Alias, GetClrMapping, GetMapping, GetSetting);
        }

        public async Task<bool> IndexAsync(PlantInfo document) => await _searchClient.IndexAsync(document);

        public async Task<BulkIndexResponse> IndexManyAsync(IEnumerable<PlantInfo> documents) => await _searchClient.IndexManyAsync(documents);

        public async Task<SearchResponse<PlantInfo>> SearchAsync(FindParams<Data.Shared.Models.PlantInfo> findParams, Data.Shared.Models.User user)
        {
            var plantInfoFindParams = findParams as PlantInfoFindParams;
            var searchTerm = findParams.SearchText;
            var shoulds = new List<QueryContainer>();
            var query = new QueryContainerDescriptor<PlantInfo>();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var fields = new FieldsDescriptor<PlantInfo>();

                fields = fields.Field(m => m.CommonName)
                        .Field(m => m.ScientificName)
                        .Field(m => m.Lifeform.CommonName)
                        .Field(m => m.Lifeform.ScientificName)
                        .Field("commonName.nameSearch")
                        .Field("scientificName.nameSearch")
                        .Field("lifeform.commonName.nameSearch")
                        .Field("lifeform.scientificName.nameSearch");

                shoulds.Add(query.MultiMatch(mm => mm.Fields(mmf => fields)
                            .Query(searchTerm)
                            .Fuzziness(Fuzziness.AutoLength(1, 3))));
                shoulds.Add(query.Nested(n => n
                            .Path(p => p.Synonyms)
                            .Query(q => q
                                .Match(sq => sq
                                    .Field("synonyms.name")
                                    .Query(searchTerm)
                                    .Fuzziness(Fuzziness.AutoLength(1, 3))))));
            }

            var searchFilters = new List<SearchFilter<PlantInfo>>
            {
                new NestedSearchValueFilter<PlantInfo, string>("Region", "location.region.keyword" ,"plantLocations", plantInfoFindParams.Filters.RegionFilter.Value),
                new SearchValuesFilter<PlantInfo, string>("Water", "waterTypes", plantInfoFindParams.Filters.WaterFilter.MinimumValue, plantInfoFindParams.Filters.WaterFilter.MaximumValue),
                new SearchValuesFilter<PlantInfo, string>("Light", "lightTypes", plantInfoFindParams.Filters.LightFilter.MinimumValue, plantInfoFindParams.Filters.LightFilter.MaximumValue),
                new SearchValuesFilter<PlantInfo, string>("Bloom", "bloomTimes", plantInfoFindParams.Filters.BloomFilter.MinimumValue?.ToString(), plantInfoFindParams.Filters.BloomFilter.MaximumValue?.ToString()),
                new NestedSearchValueFilter<PlantInfo, string>("Zone", "id", "zones", plantInfoFindParams.Filters.ZoneFilter.Value?.ToString()),
                new SearchRangeFilter<PlantInfo, double>("Height", "minHeight","maxHeight", plantInfoFindParams.Filters.HeightFilter.Values, plantInfoFindParams.Filters.HeightFilter.Value, plantInfoFindParams.Filters.HeightFilter.MaximumValue),
                new SearchRangeFilter<PlantInfo, double>("Spread", "minSpread","maxSpread", plantInfoFindParams.Filters.SpreadFilter.Values, plantInfoFindParams.Filters.SpreadFilter.Value, plantInfoFindParams.Filters.SpreadFilter.MaximumValue)
            };

            var musts = GetFilters(plantInfoFindParams, searchFilters);
            musts.Add(FilterByVisibility(query, user));

            var searchDescriptor = new SearchDescriptor<PlantInfo>()
                .Query(q => q
                    .Bool(b => b
                        .Should(shoulds.ToArray())
                        .Must(musts.ToArray()).MinimumShouldMatch(string.IsNullOrEmpty(searchTerm) ? 0 : 1)));

            var countDescriptor = new CountDescriptor<PlantInfo>()
                .Query(q => q
                    .Bool(b => b
                        .Should(shoulds.ToArray())
                        .Must(musts.ToArray()).MinimumShouldMatch(string.IsNullOrEmpty(searchTerm) ? 0 : 1)));

            var aggregations = new AggregationContainerDescriptor<PlantInfo>();

            foreach (var filter in searchFilters)
            {
                if (filter is NestedSearchValueFilter<PlantInfo, string> nestedFilter)
                {
                    aggregations = nestedFilter.ToAggregationContainerDescriptor(aggregations);
                }
                else if (filter is SearchRangeFilter<PlantInfo, double> searchRangeFilter)
                {
                    aggregations = searchRangeFilter.ToAggregationContainerDescriptor(aggregations);
                }
                else if (filter is SearchValuesFilter<PlantInfo, string> searchValuesFilter)
                {
                    aggregations = searchValuesFilter.ToAggregationContainerDescriptor(aggregations);
                }
                else if (filter is SearchValueFilter<PlantInfo, string> searchValueFilter)
                {
                    aggregations = searchValueFilter.ToAggregationContainerDescriptor(aggregations);
                }
            }

            searchDescriptor.Aggregations(a => aggregations);

            // Sort
            if (findParams.SortDirection != SortDirection.None && findParams.SortBy != null)
            {
                var plantInfoSorts = GetPlantInfoSorts();

                if (findParams.SortDirection == SortDirection.Ascending)
                {
                    searchDescriptor.Sort(s => s.Field(f => f.Field(plantInfoSorts[findParams.SortBy]).Ascending()));
                }
                else
                {
                    searchDescriptor.Sort(s => s.Field(f => f.Field(plantInfoSorts[findParams.SortBy]).Descending()));
                }
            }

            var response = await _searchClient.SearchAsync(pi => searchDescriptor.Skip(findParams.Skip).Take(findParams.Take), pi => countDescriptor);

            response.AggregationResult = ProcessAggregations(response, plantInfoFindParams);

            return response;
        }

        public async Task<SearchResponse<Lifeform>> SearchAsync(FindParams<Data.Shared.Models.Lifeform> findParams, Data.Shared.Models.User user)
        {
            var searchTerm = findParams.SearchText;
            var shoulds = new List<QueryContainer>();
            var query = new QueryContainerDescriptor<PlantInfo>();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                shoulds.Add(query.MultiMatch(mm => mm.Fields(mmf => mmf
                            .Field(m => m.CommonName)
                            .Field(m => m.ScientificName)
                            .Field(m => m.Lifeform.CommonName)
                            .Field(m => m.Lifeform.ScientificName)
                            .Field("commonName.nameSearch")
                            .Field("scientificName.nameSearch")
                            .Field("lifeform.commonName.nameSearch")
                            .Field("lifeform.scientificName.nameSearch"))
                            .Query(searchTerm)));
                shoulds.Add(query.Nested(n => n
                            .Path(p => p.Synonyms)
                            .Query(q => q
                                .Match(sq => sq
                                    .Field("synonyms.name")
                                    .Query(searchTerm)))));
            }

            var searchDescriptor = new SearchDescriptor<PlantInfo>()
                .Source(s => s.Includes(i => i.Field(p => p.Lifeform)))
                .Query(q => q
                    .Bool(b => b
                        .Should(shoulds.ToArray())));

            var countDescriptor = new CountDescriptor<PlantInfo>()
                .Query(q => q
                    .Bool(b => b
                        .Should(shoulds.ToArray())));

            var response = await _searchClient.SearchAsync(pi => searchDescriptor.Skip(findParams.Skip).Take(findParams.Take), pi => countDescriptor);

            return new SearchResponse<Lifeform>
            {
                Count = response.Count,
                Documents = response.Documents.Select(d =>
                    new Lifeform
                    {
                        Id = d.Lifeform.Id,
                        CommonName = d.Lifeform.CommonName,
                        ScientificName = d.Lifeform.ScientificName,
                    }).ToList(),
                Aggregations = null
            };
        }

        public async Task<bool> RemoveAsync(string id) => await _searchClient.RemoveAsync(id);

        public Task<bool> IndexAsync(Lifeform document) => throw new NotSupportedException();
        public Task<BulkIndexResponse> IndexManyAsync(IEnumerable<Lifeform> documents) => throw new NotSupportedException();

        private IEnumerable<AggregationResult<PlantInfo>> ProcessAggregations(SearchResponse<PlantInfo> response, PlantInfoFindParams plantInfoFindParams)
        {
            var aggregations = new List<AggregationResult<PlantInfo>>();

            foreach (var aggregation in response.Aggregations)
            {
                var bucketAggregations = new List<AggregationResult<PlantInfo>>();
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
                                bucketAggregations.Add(new AggregationResult<PlantInfo>
                                {
                                    Name = aggregation.Key,
                                    Values = bucketResults.OrderBy(v => v.Key).ToDictionary(k => k.Key, v => v.Value)
                                });
                            }
                        }
                    }
                    if (!bucketAggregations.Any())
                    {
                        var filter = PlantInfoFindParams.GetFilter(aggregation.Key, plantInfoFindParams) as Filter<string>;
                        bucketAggregations.Add(new AggregationResult<PlantInfo>
                        {
                            Name = aggregation.Key,
                            Values = new Dictionary<string, long?> { { filter.Value ?? "0", singleBucket.DocCount } }
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
                        if (bucketValue is RangeBucket rangeBucket)
                        {
                            bucketResults.Add(rangeBucket.Key.ToString(), rangeBucket.DocCount);
                        }
                        else
                        {
                            var keyedBucket = bucketValue as KeyedBucket<object>;
                            bucketResults.Add(keyedBucket.Key.ToString(), keyedBucket.DocCount);
                        }
                    }

                    bucketAggregations.Add(new AggregationResult<PlantInfo>
                    {
                        Name = aggregation.Key,
                        Values = bucketResults.OrderBy(v => v.Key).ToDictionary(k => k.Key, v => v.Value)
                    });

                    aggregations.AddRange(bucketAggregations);
                }
            }

            return aggregations;
        }

        private List<QueryContainer> GetFilters(PlantInfoFindParams findParams, List<SearchFilter<PlantInfo>> filters)
        {
            var musts = new List<QueryContainer>();
            var query = new QueryContainerDescriptor<PlantInfo>();

            if (findParams.CreatedBy != null)
            {
                musts.Add(query.Match(m => m.Field(f => f.CreatedBy).Query(findParams.CreatedBy)));
            }

            if (findParams.Filters != null)
            {
                foreach (var filter in filters)
                {
                    if (filter is NestedSearchValueFilter<PlantInfo, string> nestedFilter)
                    {
                        musts.Add(query.Bool(b => b.Should(s => nestedFilter.ToFilter(s))));
                    }
                    else if (filter is SearchRangeFilter<PlantInfo, double> searchRangeFilter)
                    {
                        musts.Add(query.Bool(b => b.Should(s => searchRangeFilter.ToFilter(s))));
                    }
                    else if (filter is SearchValuesFilter<PlantInfo, string> searchValuesFilter)
                    {
                        musts.Add(query.Bool(b => b.Should(s => searchValuesFilter.ToFilter(s))));
                    }
                    else if (filter is SearchValueFilter<PlantInfo, string> searchValueFilter)
                    {
                        musts.Add(query.Bool(b => b.Should(s => searchValueFilter.ToFilter(s))));
                    }
                }
            }

            return musts;
        }

        private QueryContainer FilterByVisibility(QueryContainerDescriptor<PlantInfo> query, Data.Shared.Models.User user) =>
            query.Bool(b => b
                    .Must(s => !s.Exists(t => t.Field(f => f.User)) ||
                                 s.Term(t => t.Visibility, Visibility.Public) ||
                                 s.Term(t => t.User.Id, user.Id) ||
                                // Not hidden
                                (!(s.Term(t => t.Visibility, Visibility.Hidden) ||
                                    (s.Term(t => t.Visibility, Visibility.Inherit) &&
                                    s.Term(t => t.User.PlantInfoVisibility, Visibility.Hidden)) ||
                                    (s.Term(t => t.User.PlantInfoVisibility, Visibility.Inherit) &&
                                    s.Term(t => t.User.ProfileVisibility, Visibility.Hidden))) &&
                                    // Inherited
                                    ((s.Term(t => t.Visibility, Visibility.Inherit) &&
                                        (s.Term(t => t.User.PlantInfoVisibility, Visibility.Public) ||
                                        (s.Term(t => t.User.PlantInfoVisibility, Visibility.Inherit) &&
                                        s.Term(t => t.User.ProfileVisibility, Visibility.Public)) ||
                                        (s.Term(t => t.User.PlantInfoVisibility, Visibility.Contacts) &&
                                        s.Term(t => t.User.ContactIds, user.Id)))) ||
                                    // Contacts
                                    (s.Term(t => t.Visibility, Visibility.Contacts) &&
                                    s.Term(t => t.User.ContactIds, user.Id))))));

        private Dictionary<string, Expression<Func<PlantInfo, object>>> GetPlantInfoSorts() => new Dictionary<string, Expression<Func<PlantInfo, object>>>
        {
            { "ScientificName", p => p.Lifeform.ScientificName.Suffix("keyword") },
            { "CommonName", p => p.Lifeform.CommonName.Suffix("keyword") },
            { "Origin", p => p.Origin.Name.Suffix("keyword") },
            { "Zone", p => p.MinimumZone.Id },
            { "Light", p => p.MinimumLight },
            { "Water", p => p.MinimumWater },
            { "BloomTime", p => p.MinimumBloomTime },
            { "Height", p => p.MinimumHeight },
            { "Spread", p => p.MinimumSpread },
            { "DateCreated", p => p.DateCreated }
        };

        private IPromise<IndexSettings> GetSetting(IndexSettingsDescriptor setting) =>
            setting.Analysis(a => a
                .Analyzers(azs => azs.Custom(NameAnalyzer, c => c.Tokenizer(NameTokenizer).Filters("lowercase")))
                .Tokenizers(t => t.EdgeNGram(NameTokenizer, ng => ng.MinGram(3).MaxGram(20).TokenChars(new[] { TokenChar.Letter }))));

        private ITypeMapping GetMapping(TypeMappingDescriptor<PlantInfo> mapping) =>
            mapping.AutoMap()
            .Properties(pi => pi
                .Nested<Synonym>(n => n
                    .Name(nn => nn.Synonyms))
                .Nested<PlantLocation>(n => n
                    .Name(nn => nn.PlantLocations))
                .Nested<Zone>(n => n
                    .Name(nn => nn.Zones))
            .Text(t => t.Name(n => n.CommonName).Fields(f => f.Text(t => t.Name("nameSearch").Analyzer(NameAnalyzer))))
            .Text(t => t.Name(n => n.ScientificName).Fields(f => f.Text(t => t.Name("nameSearch").Analyzer(NameAnalyzer))))
            .Text(t => t.Name("lifeform.commonName").Fields(f => f.Text(t => t.Name("nameSearch").Analyzer(NameAnalyzer))))
            .Text(t => t.Name("lifeform.scientificName").Fields(f => f.Text(t => t.Name("nameSearch").Analyzer(NameAnalyzer)))));

        private IClrTypeMapping<PlantInfo> GetClrMapping(ClrTypeMappingDescriptor<PlantInfo> mapping) =>
            mapping.IndexName(IndexName)
                .PropertyName(pl => pl.Id, "id")
                .PropertyName(pl => pl.CommonName, "commonName")
                .PropertyName(pl => pl.ScientificName, "scientificName")
                .PropertyName(pl => pl.Preferred, "preferred")
                .PropertyName(pl => pl.BloomTimes, "bloomTimes")
                .PropertyName(pl => pl.MinimumBloomTime, "minBloom")
                .PropertyName(pl => pl.MaximumBloomTime, "maxBloom")
                .PropertyName(pl => pl.MinimumHeight, "minHeight")
                .PropertyName(pl => pl.MaximumHeight, "maxHeight")
                .PropertyName(pl => pl.HeightUnit, "heightUnit")
                .PropertyName(pl => pl.MinimumSpread, "minSpread")
                .PropertyName(pl => pl.MaximumSpread, "maxSpread")
                .PropertyName(pl => pl.SpreadUnit, "spreadUnit")
                .PropertyName(pl => pl.WaterTypes, "waterTypes")
                .PropertyName(pl => pl.MinimumWater, "minWater")
                .PropertyName(pl => pl.MaximumWater, "maxWater")
                .PropertyName(pl => pl.LightTypes, "lightTypes")
                .PropertyName(pl => pl.MinimumLight, "minLight")
                .PropertyName(pl => pl.MaximumLight, "maxLight")
                .PropertyName(pl => pl.StratificationStages, "stratificationStages")
                .PropertyName(pl => pl.Zones, "zones")
                .PropertyName(pl => pl.MinimumZone, "minZone")
                .PropertyName(pl => pl.MaximumZone, "maxZone")
                .PropertyName(pl => pl.Visibility, "visibility")
                .PropertyName(pl => pl.CreatedBy, "createdBy")
                .PropertyName(pl => pl.ModifiedBy, "modifiedBy")
                .PropertyName(pl => pl.DateCreated, "dateCreated")
                .PropertyName(pl => pl.DateModified, "dateModified")
                .PropertyName(pl => pl.Lifeform, "lifeform")
                .PropertyName(pl => pl.Origin, "origin")
                .PropertyName(pl => pl.Taxon, "taxon")
                .PropertyName(pl => pl.User, "user")
                .PropertyName(pl => pl.PlantLocations, "plantLocations")
                .PropertyName(pl => pl.Synonyms, "synonyms");
    }
}
