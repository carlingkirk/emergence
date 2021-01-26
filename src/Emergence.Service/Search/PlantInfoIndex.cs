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
    public class PlantInfoIndex : IIndex<PlantInfo, Data.Shared.Models.PlantInfo>, IIndex<Lifeform, Data.Shared.Models.Lifeform>
    {
        private readonly ISearchClient<PlantInfo> _searchClient;
        public string IndexName => "plant_infos_01";
        public string Alias => "plant_infos";
        public string NameTokenizer => "name_tokenizer";
        public string NameAnalyzer => "name_analyzer";

        public PlantInfoIndex(ISearchClient<PlantInfo> searchClient)
        {
            _searchClient = searchClient;
            _searchClient.ConfigureClient(IndexName, Alias, GetClrMapping, GetMapping, GetSetting);
        }

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
            .Text(t => t.Name(n => n.CommonName).Fields(f => f.Text(t => t.Name("nameSearch").Analyzer(NameAnalyzer))))
            .Text(t => t.Name(n => n.ScientificName).Fields(f => f.Text(t => t.Name("nameSearch").Analyzer(NameAnalyzer))))
            .Text(t => t.Name("lifeform.commonName").Fields(f => f.Text(t => t.Name("nameSearch").Analyzer(NameAnalyzer))))
            .Text(t => t.Name("lifeform.scientificName").Fields(f => f.Text(t => t.Name("nameSearch").Analyzer(NameAnalyzer)))));

        public async Task<bool> IndexAsync(PlantInfo document) => await _searchClient.IndexAsync(document);
        public async Task<BulkIndexResponse> IndexManyAsync(IEnumerable<PlantInfo> documents) => await _searchClient.IndexManyAsync(documents);

        public async Task<SearchResponse<PlantInfo>> SearchAsync(FindParams<Data.Shared.Models.PlantInfo> findParams, Data.Shared.Models.User user)
        {
            var searchTerm = findParams.SearchText;
            var musts = GetFilters(findParams);
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
                            .Query(searchTerm)
                            .Fuzziness(Fuzziness.AutoLength(1, 5))));
                shoulds.Add(query.Nested(n => n
                            .Path(p => p.Synonyms)
                            .Query(q => q
                                .Match(sq => sq
                                    .Field("synonyms.name")
                                    .Query(searchTerm)
                                    .Fuzziness(Fuzziness.AutoLength(1, 5))))));
            }

            musts.Add(FilterByVisibility(query, user));

            var searchDescriptor = new SearchDescriptor<PlantInfo>()
                .Query(q => q
                    .Bool(b => b
                        .Should(shoulds.ToArray())
                        .Must(musts.ToArray())));

            var countDescriptor = new CountDescriptor<PlantInfo>()
                .Query(q => q
                    .Bool(b => b
                        .Should(shoulds.ToArray())
                        .Must(musts.ToArray())));

            // Sort
            if (findParams.SortDirection != SortDirection.None)
            {
                if (findParams.SortBy == null)
                {
                    findParams.SortBy = "DateCreated";
                }

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

            // Aggregations
            searchDescriptor.Aggregations(a => a.Nested("Region", n => n.Path("plantLocations").Aggregations(a => a.Terms("Region", t => t.Field("plantLocations.location.region.keyword")))));


            var response = await _searchClient.SearchAsync(pi => searchDescriptor.Skip(findParams.Skip).Take(findParams.Take), pi => countDescriptor);

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

        private List<QueryContainer> GetFilters(FindParams<Data.Shared.Models.PlantInfo> findParams)
        {
            var musts = new List<QueryContainer>();
            var query = new QueryContainerDescriptor<PlantInfo>();
            var plantInfoFindParams = findParams as PlantInfoFindParams;
            if (findParams.CreatedBy != null)
            {
                musts.Add(query.Match(m => m.Field(f => f.CreatedBy).Query(findParams.CreatedBy)));
            }

            if (plantInfoFindParams.Filters != null)
            {
                var filters = plantInfoFindParams.Filters;

                var heightFilter = filters.HeightFilter;

                if (heightFilter.MinimumValue > 0)
                {
                    musts.Add(query.Range(r => r.Field(f => f.MinimumHeight).GreaterThanOrEquals(heightFilter.MinimumValue)));
                }

                if (heightFilter.MaximumValue > 0)
                {
                    musts.Add(query.Range(r => r.Field(f => f.MaximumHeight).LessThanOrEquals(heightFilter.MaximumValue)));
                }

                var regionFilter = filters.RegionFilter;
                if (!string.IsNullOrEmpty(regionFilter.Value))
                {
                    musts.Add(query.Nested(n => n
                        .Path(p => p.PlantLocations)
                            .Query(q => q
                                .Match(sq => sq
                                    .Field("plantLocations.location.region")
                                    .Query(regionFilter.Value)
                                    .Fuzziness(Fuzziness.AutoLength(1, 5))))));
                }

                var spreadFilter = filters.SpreadFilter;
                if (spreadFilter.MinimumValue > 0)
                {
                    musts.Add(query.Range(r => r.Field(f => f.MinimumSpread).GreaterThanOrEquals(spreadFilter.MinimumValue)));
                }

                if (spreadFilter.MaximumValue > 0)
                {
                    musts.Add(query.Range(r => r.Field(f => f.MaximumSpread).LessThanOrEquals(spreadFilter.MaximumValue)));
                }

                var lightFilter = filters.LightFilter;
                if (!string.IsNullOrEmpty(lightFilter.MinimumValue))
                {
                    var minLightValue = (double)Enum.Parse<LightType>(lightFilter.MinimumValue);
                    musts.Add(query.Bool(b => b.Should(s => s.Range(r => r.Field(f => f.MinimumLight).GreaterThanOrEquals(minLightValue)) ||
                                                           (!s.Exists(e => e.Field(f => f.MinimumLight)) &&
                                                             s.Exists(e => e.Field(f => f.MaximumLight))))));
                }

                if (!string.IsNullOrEmpty(lightFilter.MaximumValue))
                {
                    var maxLightValue = (double)Enum.Parse<LightType>(lightFilter.MaximumValue);
                    musts.Add(query.Bool(b => b.Should(s => s.Range(r => r.Field(f => f.MaximumLight).LessThanOrEquals(maxLightValue)) ||
                                                           (!s.Exists(e => e.Field(f => f.MaximumLight)) &&
                                                             s.Exists(e => e.Field(f => f.MinimumLight))))));
                }

                var waterFilter = filters.WaterFilter;
                if (!string.IsNullOrEmpty(waterFilter.MinimumValue))
                {
                    var minWaterValue = (double)Enum.Parse<WaterType>(waterFilter.MinimumValue);
                    musts.Add(query.Bool(b => b.Should(s => s.Range(r => r.Field(f => f.MinimumWater).GreaterThanOrEquals(minWaterValue)) ||
                                                           (!s.Exists(e => e.Field(f => f.MinimumWater)) &&
                                                             s.Exists(e => e.Field(f => f.MaximumWater))))));
                }

                if (!string.IsNullOrEmpty(waterFilter.MaximumValue))
                {
                    var maxWaterValue = (double)Enum.Parse<WaterType>(waterFilter.MaximumValue);
                    musts.Add(query.Bool(b => b.Should(s => s.Range(r => r.Field(f => f.MaximumWater).LessThanOrEquals(maxWaterValue)) ||
                                                           (!s.Exists(e => e.Field(f => f.MaximumWater)) &&
                                                             s.Exists(e => e.Field(f => f.MinimumWater))))));
                }

                var bloomFilter = filters.BloomFilter;

                if (bloomFilter.MinimumValue > 0 && bloomFilter.MaximumValue > 0)
                {
                    if (bloomFilter.MaximumValue >= bloomFilter.MinimumValue)
                    {
                        musts.Add(query.Bool(b => b.Should(s => s.Range(r => r.Field(f => f.MinimumBloomTime)
                                                                    .LessThanOrEquals(bloomFilter.MaximumValue)
                                                                    .GreaterThanOrEquals(bloomFilter.MinimumValue)) ||
                                                                s.Range(r => r.Field(f => f.MaximumBloomTime)
                                                                    .LessThanOrEquals(bloomFilter.MaximumValue)
                                                                    .GreaterThanOrEquals(bloomFilter.MinimumValue)) ||
                                                                (s.Range(r => r.Field(f => f.MinimumBloomTime).GreaterThanOrEquals(bloomFilter.MinimumValue)) &&
                                                                 !s.Exists(e => e.Field(f => f.MaximumBloomTime))) ||
                                                                (s.Range(r => r.Field(f => f.MaximumBloomTime).LessThanOrEquals(bloomFilter.MaximumValue)) &&
                                                                 !s.Exists(e => e.Field(f => f.MinimumBloomTime))))));
                    }
                    else
                    {
                        // client sent in months that span two years
                        musts.Add(query.Bool(b => b.Should(s => (s.Range(r => r.Field(f => f.MinimumBloomTime).GreaterThanOrEquals(bloomFilter.MinimumValue)) &&
                                                                 !s.Exists(e => e.Field(f => f.MaximumBloomTime))) ||
                                                                (s.Range(r => r.Field(f => f.MaximumBloomTime).LessThanOrEquals(bloomFilter.MaximumValue)) ||
                                                                 s.Range(r => r.Field(f => f.MinimumBloomTime).LessThanOrEquals(bloomFilter.MaximumValue))))));
                    }
                }
                else
                {
                    if (bloomFilter.MinimumValue > 0)
                    {
                        musts.Add(query.Bool(b => b.Should(s => s.Range(r => r.Field(f => f.MinimumBloomTime).GreaterThanOrEquals(bloomFilter.MinimumValue)) ||
                                                                (!s.Exists(e => e.Field(f => f.MinimumBloomTime)) &&
                                                                  s.Exists(e => e.Field(f => f.MaximumBloomTime))))));
                    }

                    if (bloomFilter.MaximumValue > 0)
                    {
                        musts.Add(query.Bool(b => b.Should(s => s.Range(r => r.Field(f => f.MaximumBloomTime).LessThanOrEquals(bloomFilter.MaximumValue)) ||
                                                                (!s.Exists(e => e.Field(f => f.MaximumBloomTime)) &&
                                                                  s.Exists(e => e.Field(f => f.MinimumBloomTime))))));
                    }
                }

                var zoneFilter = filters.ZoneFilter;
                if (zoneFilter.Value > 0)
                {
                    musts.Add(query.Bool(b => b.Should(s => (s.Exists(e => e.Field(f => f.MinimumZone)) ||
                                                             s.Exists(e => e.Field(f => f.MaximumZone))) &&
                                                            (s.Range(r => r.Field(f => f.MinimumZone.Id).LessThanOrEquals(zoneFilter.Value)) ||
                                                             !s.Exists(e => e.Field(f => f.MinimumZone))) &&
                                                            (s.Range(r => r.Field(f => f.MaximumZone.Id).GreaterThanOrEquals(zoneFilter.Value)) ||
                                                             !s.Exists(e => e.Field(f => f.MaximumZone))))));
                }
            }

            return musts;
        }

        private QueryContainer FilterByVisibility(QueryContainerDescriptor<PlantInfo> query, Data.Shared.Models.User user) =>
            query.Bool(b => b
                    .Should(s => !s.Exists(t => t.Field(f => f.User)) ||
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

        private IClrTypeMapping<PlantInfo> GetClrMapping(ClrTypeMappingDescriptor<PlantInfo> mapping) =>
            mapping.IndexName(IndexName)
                .PropertyName(pl => pl.Id, "id")
                .PropertyName(pl => pl.CommonName, "commonName")
                .PropertyName(pl => pl.ScientificName, "scientificName")
                .PropertyName(pl => pl.Preferred, "preferred")
                .PropertyName(pl => pl.MinimumBloomTime, "minBloom")
                .PropertyName(pl => pl.MaximumBloomTime, "maxBloom")
                .PropertyName(pl => pl.MinimumHeight, "minHeight")
                .PropertyName(pl => pl.MaximumHeight, "maxHeight")
                .PropertyName(pl => pl.HeightUnit, "heightUnit")
                .PropertyName(pl => pl.MinimumSpread, "minSpread")
                .PropertyName(pl => pl.MaximumSpread, "maxSpread")
                .PropertyName(pl => pl.SpreadUnit, "spreadUnit")
                .PropertyName(pl => pl.MinimumWater, "minWater")
                .PropertyName(pl => pl.MaximumWater, "maxWater")
                .PropertyName(pl => pl.MinimumLight, "minLight")
                .PropertyName(pl => pl.MaximumLight, "maxLight")
                .PropertyName(pl => pl.StratificationStages, "stratificationStages")
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

        public Task<bool> IndexAsync(Lifeform document) => throw new NotSupportedException();
        public Task<BulkIndexResponse> IndexManyAsync(IEnumerable<Lifeform> documents) => throw new NotSupportedException();
    }
}
