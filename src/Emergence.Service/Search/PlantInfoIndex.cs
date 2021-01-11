using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Search;
using Emergence.Data.Shared.Search.Models;
using Nest;

namespace Emergence.Service.Search
{
    public class PlantInfoIndex : IIndex<PlantInfo>
    {
        private readonly ISearchClient<PlantInfo> _searchClient;
        public string IndexName => "plant_infos";


        public PlantInfoIndex(ISearchClient<PlantInfo> searchClient)
        {
            _searchClient = searchClient;
            _searchClient.ConfigureClient(IndexName, GetClrMapping, GetMapping);
        }

        private ITypeMapping GetMapping(TypeMappingDescriptor<PlantInfo> mapping) =>
            mapping.AutoMap()
            .Properties(pi => pi
                .Nested<Synonym>(n => n
                    .Name(nn => nn.Synonyms))
                .Nested<PlantLocation>(n => n
                    .Name(nn => nn.PlantLocations))
            );

        public async Task<bool> IndexAsync(PlantInfo document) => await _searchClient.IndexAsync(document);
        public async Task<BulkIndexResponse> IndexManyAsync(IEnumerable<PlantInfo> documents) => await _searchClient.IndexManyAsync(documents);

        public async Task<SearchResponse<PlantInfo>> SearchAsync(FindParams findParams)
        {
            var searchTerm = findParams.SearchText;
            var query = new QueryContainerDescriptor<PlantInfo>();
            var musts = new List<QueryContainer>();
            var shoulds = new List<QueryContainer>();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                shoulds.Add(query.MultiMatch(mm => mm.Fields(mmf => mmf
                            .Field(m => m.CommonName)
                            .Field(m => m.ScientificName)
                            .Field(m => m.Lifeform.CommonName)
                            .Field(m => m.Lifeform.ScientificName))
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

            if (findParams.CreatedBy != null)
            {
                musts.Add(query.Match(m => m.Field(f => f.CreatedBy).Query(findParams.CreatedBy)));
            }

            foreach (var filter in findParams.Filters)
            {
                if (filter.Name == "Height")
                {
                    var heightFilter = new HeightFilter((RangeFilter<double?>)filter);

                    if (heightFilter.MinimumValue.HasValue)
                    {
                        musts.Add(query.Range(r => r.Field(f => f.MinimumHeight).GreaterThanOrEquals(heightFilter.MinimumValue)));
                    }

                    if (heightFilter.MaximumValue.HasValue)
                    {
                        musts.Add(query.Range(r => r.Field(f => f.MaximumHeight).LessThanOrEquals(heightFilter.MaximumValue)));
                    }
                }
                else if (filter.Name == "Location")
                {
                    var regionFilter = new RegionFilter((Filter<string>)filter);
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
                }
                //else if (filter.Name == "Spread")
                //{
                //    var spreadFilter = new SpreadFilter((RangeFilter<double?>)filter);
                //    if (spreadFilter.MinimumValue.HasValue || spreadFilter.MaximumValue.HasValue)
                //    {
                //        plantInfoQuery = plantInfoQuery.Where(spreadFilter.Filter);
                //    }
                //}
                //else if (filter.Name == "Light")
                //{
                //    var lightFilter = new LightFilter((RangeFilter<string>)filter);
                //    if (!(string.IsNullOrEmpty(lightFilter.MinimumValue) && string.IsNullOrEmpty(lightFilter.MaximumValue)))
                //    {
                //        plantInfoQuery = plantInfoQuery.Where(lightFilter.Filter);
                //    }
                //}
                //else if (filter.Name == "Water")
                //{
                //    var waterFilter = new WaterFilter((RangeFilter<string>)filter);
                //    if (!(string.IsNullOrEmpty(waterFilter.MinimumValue) && string.IsNullOrEmpty(waterFilter.MaximumValue)))
                //    {
                //        plantInfoQuery = plantInfoQuery.Where(waterFilter.Filter);
                //    }
                //}
                //else if (filter.Name == "Bloom")
                //{
                //    var bloomFilter = new BloomFilter((RangeFilter<int>)filter);
                //    if (!(bloomFilter.MinimumValue == 0 && bloomFilter.MaximumValue == 0))
                //    {
                //        plantInfoQuery = plantInfoQuery.Where(bloomFilter.Filter);
                //    }
                //}
                //else if (filter.Name == "Zone")
                //{
                //    var zoneFilter = new ZoneFilter((SelectFilter<int>)filter);
                //    if (zoneFilter.Value > 0)
                //    {
                //        plantInfoQuery = plantInfoQuery.Where(zoneFilter.Filter);
                //    }
                //}
            }

            var response = await _searchClient.SearchAsync(pi => pi
            .Bool(b => b
                .Should(shoulds.ToArray())
                .Must(musts.ToArray())), 0, 10);

            return response;
        }

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
    }
}
