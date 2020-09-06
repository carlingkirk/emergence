using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Service.Interfaces;
using Models = Emergence.Data.Shared.Models;

namespace Emergence.Transform
{
    public class PlantInfoProcessor : IPlantInfoProcessor
    {
        private readonly ILifeformService _lifeformService;
        private readonly IOriginService _originService;
        private readonly IPlantInfoService _plantInfoService;
        private readonly ITaxonService _taxonService;
        private readonly ILocationService _locationService;

        private Models.Origin Origin;
        private List<Models.Lifeform> Lifeforms { get; set; }
        private List<Models.Taxon> Taxons { get; set; }

        public PlantInfoProcessor(ILifeformService lifeformService, IOriginService originService, IPlantInfoService plantInfoService, ITaxonService taxonService, ILocationService locationService)
        {
            _lifeformService = lifeformService;
            _originService = originService;
            _plantInfoService = plantInfoService;
            _taxonService = taxonService;
            _locationService = locationService;
        }

        public async Task InitializeOrigin(Models.Origin origin)
        {
            Origin = await _originService.GetOriginAsync(origin.OriginId);
            if (Origin == null)
            {
                Origin = await _originService.AddOrUpdateOriginAsync(origin, null);
            }
        }

        public async Task InitializeLifeforms()
        {
            var lifeformResult = await _lifeformService.GetLifeformsAsync();
            Lifeforms = lifeformResult.ToList();
        }

        public async Task InitializeTaxons()
        {
            var taxonResult = await _taxonService.GetTaxonsAsync();
            Taxons = taxonResult.ToList();
        }

        public async Task<Models.PlantInfo> Process(Models.PlantInfo plantInfo)
        {
            var lifeform = Lifeforms.FirstOrDefault(l => l.ScientificName == plantInfo.ScientificName);

            if (lifeform == null)
            {
                lifeform = await _lifeformService.AddOrUpdateLifeformAsync(plantInfo.Lifeform);
            }

            var taxon = Taxons.FirstOrDefault(t => t.Genus == plantInfo.Taxon.Genus && t.Species == plantInfo.Taxon.Species &&
                                                                (plantInfo.Taxon.Subspecies == null || t.Subspecies == plantInfo.Taxon.Subspecies) &&
                                                                (plantInfo.Taxon.Variety == null || t.Variety == plantInfo.Taxon.Variety) &&
                                                                (plantInfo.Taxon.Subvariety == null || t.Subvariety == plantInfo.Taxon.Subvariety) &&
                                                                (plantInfo.Taxon.Form == null || t.Form == plantInfo.Taxon.Form));

            if (taxon == null)
            {
                taxon = await _taxonService.AddOrUpdateTaxonAsync(plantInfo.Taxon);
            }

            var originResult = await _originService.GetOriginAsync(Origin.OriginId, plantInfo.Origin.ExternalId, plantInfo.Origin.AltExternalId);
            if (originResult == null)
            {
                originResult = await _originService.AddOrUpdateOriginAsync(plantInfo.Origin, null);
            }

            var plantInfoResult = await _plantInfoService.GetPlantInfoAsync(originResult.OriginId, taxon.TaxonId);

            if (plantInfoResult == null)
            {
                plantInfo.Origin = originResult;
                plantInfo.Lifeform = lifeform;
                plantInfo.Taxon = taxon;
                plantInfoResult = await _plantInfoService.AddOrUpdatePlantInfoAsync(plantInfo);
            }

            return plantInfoResult;
        }

        public async Task<IEnumerable<Models.PlantInfo>> Process(IEnumerable<Models.PlantInfo> plantInfos)
        {
            var plantInfoResults = new List<Models.PlantInfo>();
            var newOrigins = new List<Models.Origin>();
            var newPlantInfos = new List<Models.PlantInfo>();
            foreach (var plantInfo in plantInfos)
            {
                var lifeform = Lifeforms.FirstOrDefault(l => l.ScientificName == plantInfo.ScientificName);

                if (lifeform == null)
                {
                    lifeform = await _lifeformService.AddOrUpdateLifeformAsync(plantInfo.Lifeform);
                }
                plantInfo.Lifeform = lifeform;

                var taxon = Taxons.FirstOrDefault(t => t.Genus == plantInfo.Taxon.Genus && t.Species == plantInfo.Taxon.Species &&
                                                                    (plantInfo.Taxon.Subspecies == null || t.Subspecies == plantInfo.Taxon.Subspecies) &&
                                                                    (plantInfo.Taxon.Variety == null || t.Variety == plantInfo.Taxon.Variety) &&
                                                                    (plantInfo.Taxon.Subvariety == null || t.Subvariety == plantInfo.Taxon.Subvariety) &&
                                                                    (plantInfo.Taxon.Form == null || t.Form == plantInfo.Taxon.Form));

                if (taxon == null || taxon.Kingdom != plantInfo.Taxon.Kingdom || taxon.Family != plantInfo.Taxon.Family)
                {
                    taxon = await _taxonService.AddOrUpdateTaxonAsync(plantInfo.Taxon);
                }
                plantInfo.Taxon = taxon;

                var originResult = await _originService.GetOriginAsync(Origin.OriginId, plantInfo.Origin.ExternalId, plantInfo.Origin.AltExternalId);
                newOrigins.Add(plantInfo.Origin);
            }

            if (newOrigins.Any())
            {
                newOrigins = (await _originService.AddOriginsAsync(newOrigins)).ToList();
            }

            foreach (var plantInfo in plantInfos)
            {
                var plantInfoResult = await _plantInfoService.GetPlantInfoAsync(plantInfo.Origin.OriginId, plantInfo.Taxon.TaxonId);

                if (plantInfoResult == null)
                {
                    var origin = newOrigins.FirstOrDefault(o => o.ParentOrigin.OriginId == plantInfo.Origin.OriginId
                                                             && o.ExternalId == plantInfo.Origin.ExternalId
                                                             && o.AltExternalId == plantInfo.Origin.AltExternalId);
                    if (origin == null)
                    {
                        origin = await _originService.GetOriginAsync(Origin.OriginId, plantInfo.Origin.ExternalId, plantInfo.Origin.AltExternalId);
                    }

                    plantInfo.Origin = origin;

                    newPlantInfos.Add(plantInfo);
                }
            }

            if (newPlantInfos.Any())
            {
                newPlantInfos = (await _plantInfoService.AddPlantInfosAsync(newPlantInfos)).ToList();
            }

            if (newPlantInfos.Any(p => p.Locations.Any()))
            {
                var regions = newPlantInfos.SelectMany(p => p.Locations).Select(l => l.Location.Region).Distinct();
                var countries = newPlantInfos.SelectMany(p => p.Locations).Select(l => l.Location.Country).Distinct();
                var locations = _locationService.GetLocationsAsync(l => countries.Contains(l.Country) || regions.Contains(l.Region));
            }

            return newPlantInfos;
        }
    }
}
