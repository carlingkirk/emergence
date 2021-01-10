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
            _searchClient.ConfigureClient(IndexName, GetMapping);
        }

        public async Task CreateIndexAsync() => await _searchClient.CreateIndexAsync("plant_infos");
        public async Task<bool> IndexAsync(PlantInfo document) => await _searchClient.IndexAsync(document);
        public async Task<BulkIndexResponse> IndexManyAsync(IEnumerable<PlantInfo> documents) => await _searchClient.IndexManyAsync(documents);

        private IClrTypeMapping<PlantInfo> GetMapping(ClrTypeMappingDescriptor<PlantInfo> mapping) =>
            mapping.IndexName(IndexName)
                .PropertyName(pl => pl.Id, "id")
                .PropertyName(pl => pl.CommonName, "common_name")
                .PropertyName(pl => pl.ScientificName, "scientific_name")
                .PropertyName(pl => pl.Preferred, "preferred")
                .PropertyName(pl => pl.MinimumBloomTime, "min_bloom")
                .PropertyName(pl => pl.MaximumBloomTime, "max_bloom")
                .PropertyName(pl => pl.MinimumHeight, "min_height")
                .PropertyName(pl => pl.MaximumHeight, "max_height")
                .PropertyName(pl => pl.HeightUnit, "height_unit")
                .PropertyName(pl => pl.MinimumSpread, "min_spread")
                .PropertyName(pl => pl.MaximumSpread, "max_spread")
                .PropertyName(pl => pl.SpreadUnit, "spread_unit")
                .PropertyName(pl => pl.MinimumWater, "min_water")
                .PropertyName(pl => pl.MaximumWater, "max_water")
                .PropertyName(pl => pl.MinimumLight, "min_light")
                .PropertyName(pl => pl.MaximumLight, "max_light")
                .PropertyName(pl => pl.StratificationStages, "stratification_stages")
                .PropertyName(pl => pl.MinimumZone, "min_zone")
                .PropertyName(pl => pl.MaximumZone, "max_zone")
                .PropertyName(pl => pl.Visibility, "visibility")
                .PropertyName(pl => pl.CreatedBy, "created_by")
                .PropertyName(pl => pl.ModifiedBy, "modified_by")
                .PropertyName(pl => pl.DateCreated, "date_created")
                .PropertyName(pl => pl.DateModified, "date_modified")
                .PropertyName(pl => pl.Lifeform, "lifeform")
                .PropertyName(pl => pl.Origin, "origin")
                .PropertyName(pl => pl.Taxon, "taxon")
                .PropertyName(pl => pl.User, "user")
                .PropertyName(pl => pl.PlantLocations, "plant_locations")
                .PropertyName(pl => pl.Synonyms, "synonyms");
    }

    
}
