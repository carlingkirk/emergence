using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<SearchResponse<PlantInfo>> SearchAsync(string search)
        {
            var query = new QueryContainerDescriptor<PlantInfo>();
            var should = new List<QueryContainer>
            {
                query.MultiMatch(mm => mm.Fields(mmf => mmf
                        .Field(m => m.CommonName)
                        .Field(m => m.ScientificName)
                        .Field(m => m.Lifeform.CommonName)
                        .Field(m => m.Lifeform.ScientificName))
                        .Query(search)
                        .Fuzziness(Fuzziness.AutoLength(1, 5))),
                query.Nested(n => n
                        .Path(p => p.Synonyms)
                        .Query(q => q
                            .Match(sq => sq
                                .Field("synonyms.name")
                                .Query(search)
                                .Fuzziness(Fuzziness.AutoLength(1, 5)))))
            };

            var response = await _searchClient.SearchAsync(pi => pi
            .Bool(b => b
                .Must(m => m
                    .Bool(mb => mb
                        .Should(should.ToArray())))), 0, 10);

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
