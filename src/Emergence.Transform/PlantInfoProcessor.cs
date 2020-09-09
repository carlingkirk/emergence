using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;
using Emergence.Service.Extensions;
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

        private Origin Origin;
        private List<Lifeform> Lifeforms { get; set; }
        private List<Taxon> Taxons { get; set; }
        private List<Origin> Origins { get; set; }

        public PlantInfoProcessor(ILifeformService lifeformService, IOriginService originService, IPlantInfoService plantInfoService, ITaxonService taxonService, ILocationService locationService)
        {
            _lifeformService = lifeformService;
            _originService = originService;
            _plantInfoService = plantInfoService;
            _taxonService = taxonService;
            _locationService = locationService;

            Lifeforms = new List<Lifeform>();
            Taxons = new List<Taxon>();
            Origins = new List<Origin>();
        }

        public async Task InitializeOrigin(Origin origin)
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

        public async Task<PlantInfo> Process(PlantInfo plantInfo)
        {
            var lifeform = Lifeforms.FirstOrDefault(l => l.ScientificName == plantInfo.ScientificName);

            if (lifeform == null)
            {
                lifeform = await _lifeformService.AddOrUpdateLifeformAsync(plantInfo.Lifeform);
                Lifeforms.Add(lifeform);
            }

            var taxon = Taxons.FirstOrDefault(t => t.Genus == plantInfo.Taxon.Genus && t.Species == plantInfo.Taxon.Species &&
                                                                (plantInfo.Taxon.Subspecies == null || t.Subspecies == plantInfo.Taxon.Subspecies) &&
                                                                (plantInfo.Taxon.Variety == null || t.Variety == plantInfo.Taxon.Variety) &&
                                                                (plantInfo.Taxon.Subvariety == null || t.Subvariety == plantInfo.Taxon.Subvariety) &&
                                                                (plantInfo.Taxon.Form == null || t.Form == plantInfo.Taxon.Form));

            if (taxon == null)
            {
                taxon = await _taxonService.AddOrUpdateTaxonAsync(plantInfo.Taxon);
                Taxons.Add(taxon);
            }

            var originResult = Origins.FirstOrDefault(o => o.ExternalId == plantInfo.Origin.ExternalId &&
                                                           o.AltExternalId == plantInfo.Origin.AltExternalId);
            if (originResult == null)
            {
                originResult = await _originService.GetOriginAsync(Origin.OriginId, plantInfo.Origin.ExternalId, plantInfo.Origin.AltExternalId);
                Origins.Add(originResult);
            }

            if (originResult == null)
            {
                originResult = await _originService.AddOrUpdateOriginAsync(plantInfo.Origin, null);
                Origins.Add(originResult);
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

        public async Task<IEnumerable<PlantInfo>> Process(IEnumerable<Models.PlantInfo> plantInfos)
        {
            var plantInfoResults = new List<PlantInfo>();
            var newOrigins = new List<Origin>();
            var newPlantInfos = new List<PlantInfo>();
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

                // Do we already have the same origin in our insert list?
                var originResult = newOrigins.FirstOrDefault(o => o.ExternalId == plantInfo.Origin.ExternalId
                                                               && o.AltExternalId == plantInfo.Origin.AltExternalId);
                if (originResult == null)
                {
                    // See if it already exists, if not, add it to the insert list
                    originResult = await _originService.GetOriginAsync(Origin.OriginId, plantInfo.Origin.ExternalId, plantInfo.Origin.AltExternalId);
                    if (originResult == null)
                    {
                        newOrigins.Add(plantInfo.Origin);
                    }
                    else
                    {
                        Origins.Add(originResult);
                    }
                }
            }

            if (newOrigins.Any())
            {
                newOrigins = (await _originService.AddOriginsAsync(newOrigins)).ToList();
                Origins.AddRange(newOrigins);
            }

            foreach (var plantInfo in plantInfos)
            {
                var plantInfoResult = await _plantInfoService.GetPlantInfoAsync(plantInfo.Origin.OriginId, plantInfo.Taxon.TaxonId);

                if (plantInfoResult == null)
                {
                    var origin = Origins.FirstOrDefault(o => o.ExternalId == plantInfo.Origin.ExternalId
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

            var plantLocationsResult = new List<PlantLocation>();
            var plantInfoLocations = plantInfos.Where(p => p.Locations != null && p.Locations.Any()).SelectMany(p => p.Locations).ToList();

            if (plantInfoLocations.Any())
            {
                var regions = plantInfos.SelectMany(p => p.Locations).Select(l => l.Location.Region).Distinct();
                var countries = plantInfos.SelectMany(p => p.Locations).Select(l => l.Location.Country).Distinct();
                var locations = (await _locationService.GetLocationsAsync(l => countries.Contains(l.Country) || regions.Contains(l.Region))).ToList();

                var missingLocations = new List<Location>();
                missingLocations = plantInfoLocations.GroupJoin(locations,
                    pl => new { pl.Location.Region, pl.Location.Country },
                    l => new { l.Region, l.Country },
                    (pl, l) => pl.Location)
                    .DistinctBy(l => new { l.Region, l.Country })
                    .ToList();

                if (missingLocations.Any())
                {
                    var locationResult = await _locationService.AddLocationsAsync(missingLocations);
                    locations.AddRange(locationResult.ToList());
                }

                var plantLocations = new List<PlantLocation>();
                foreach (var plantInfoLocation in plantInfoLocations)
                {
                    var newPlantInfo = newPlantInfos.Where(npl => npl.Origin.OriginId == plantInfoLocation.PlantInfo.Origin.OriginId
                                                        && npl.Taxon.TaxonId == plantInfoLocation.PlantInfo.Taxon.TaxonId).First();
                    var location = locations.Where(l => l.Country == plantInfoLocation.Location.Country
                                                        && l.Region == plantInfoLocation.Location.Region).First();
                    plantLocations.Add(new PlantLocation
                    {
                        PlantInfo = newPlantInfo,
                        Location = location,
                        Status = plantInfoLocation.Status
                    });
                }

                plantLocationsResult = (await _plantInfoService.AddPlantLocations(plantLocations)).ToList();
            }

            foreach (var newPlantInfo in newPlantInfos)
            {
                var plantInfo = plantInfos.First(p => p.Origin.OriginId == newPlantInfo.Origin.OriginId
                                                        && p.Taxon.TaxonId == newPlantInfo.Taxon.TaxonId);
                var plantLocations = plantLocationsResult.Where(pl => pl.PlantInfo.PlantInfoId == newPlantInfo.PlantInfoId);
                newPlantInfo.Locations = plantLocations.Any() ? plantLocations : null;
                newPlantInfo.Taxon = plantInfo.Taxon;
                newPlantInfo.Origin = plantInfo.Origin;
            }

            return newPlantInfos;
        }
    }
}
