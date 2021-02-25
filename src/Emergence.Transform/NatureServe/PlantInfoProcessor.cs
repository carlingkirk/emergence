using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;
using Emergence.Service.Extensions;
using Emergence.Service.Interfaces;

namespace Emergence.Transform.NatureServe
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
            var itisDate = new DateTime(2020, 08, 25);
            var taxonResult = await _taxonService.GetTaxonsAsync(t => t.DateCreated >= itisDate);
            Taxons = taxonResult.ToList();
        }

        public async Task<PlantInfo> Process(PlantInfo plantInfo)
        {
            var lifeform = Lifeforms.FirstOrDefault(l => l.ScientificName == plantInfo.ScientificName);

            if (lifeform == null)
            {
                throw new Exception("We shouldn't have any missing lifeforms at this point.");
            }

            var taxon = Taxons.FirstOrDefault(t => t.Kingdom == plantInfo.Taxon.Kingdom
                                                && t.Subkingdom == plantInfo.Taxon.Subkingdom
                                                && t.Infrakingdom == plantInfo.Taxon.Infrakingdom
                                                && t.Phylum == plantInfo.Taxon.Phylum
                                                && t.Subphylum == plantInfo.Taxon.Subphylum
                                                && t.Class == plantInfo.Taxon.Class
                                                && t.Subclass == plantInfo.Taxon.Subclass
                                                && t.Superorder == plantInfo.Taxon.Superorder
                                                && t.Order == plantInfo.Taxon.Order
                                                && t.Family == plantInfo.Taxon.Family
                                                && t.Genus == plantInfo.Taxon.Genus
                                                && t.Species == plantInfo.Taxon.Species
                                                && (t.Subspecies == plantInfo.Taxon.Subspecies || plantInfo.Taxon.Variety != null)
                                                && t.Variety == plantInfo.Taxon.Variety
                                                && t.Subvariety == plantInfo.Taxon.Subvariety
                                                && t.Form == plantInfo.Taxon.Form);

            if (taxon == null)
            {
                throw new Exception("We shouldn't have any missing taxa at this point.");
            }

            var originResult = Origins.FirstOrDefault(o => o.ParentOrigin.OriginId == Origin.OriginId
                                                        && o.ExternalId == plantInfo.Origin.ExternalId
                                                        && o.AltExternalId == plantInfo.Origin.AltExternalId);
            if (originResult == null)
            {
                originResult = await _originService.GetOriginAsync(Origin.OriginId, plantInfo.Origin.ExternalId, plantInfo.Origin.AltExternalId);
            }

            if (originResult != null)
            {
                Origins.Add(originResult);
            }
            else
            {
                //originResult = await _originService.AddOrUpdateOriginAsync(plantInfo.Origin, null);
                //Origins.Add(originResult);
            }

            var plantInfoResult = await _plantInfoService.GetPlantInfoAsync(originResult.OriginId, taxon.TaxonId);

            if (plantInfoResult == null)
            {
                plantInfo.Origin = originResult;
                plantInfo.Lifeform = lifeform;
                plantInfo.Taxon = taxon;
                //plantInfoResult = await _plantInfoService.AddOrUpdatePlantInfoAsync(plantInfo);
            }

            return plantInfoResult;
        }

        public async Task<IEnumerable<PlantInfo>> Process(IEnumerable<PlantInfo> plantInfos)
        {
            var newOrigins = new List<Origin>();
            var newPlantInfos = new List<PlantInfo>();
            foreach (var plantInfo in plantInfos)
            {
                var lifeform = Lifeforms.FirstOrDefault(l => l.ScientificName == plantInfo.ScientificName);

                plantInfo.Lifeform = lifeform ?? throw new Exception("We shouldn't have any missing lifeforms at this point.");

                var taxon = Taxons.FirstOrDefault(t => t.Kingdom == plantInfo.Taxon.Kingdom
                                                && t.Phylum == plantInfo.Taxon.Phylum
                                                && t.Class == plantInfo.Taxon.Class
                                                && t.Order == plantInfo.Taxon.Order
                                                && t.Family == plantInfo.Taxon.Family
                                                && t.Genus == plantInfo.Taxon.Genus
                                                && t.Species == plantInfo.Taxon.Species
                                                && (t.Subspecies == plantInfo.Taxon.Subspecies || !string.IsNullOrEmpty(plantInfo.Taxon.Variety))
                                                && t.Variety == plantInfo.Taxon.Variety
                                                && t.Subvariety == plantInfo.Taxon.Subvariety
                                                && t.Form == plantInfo.Taxon.Form);

                if (taxon == null)
                {
                    taxon = await _taxonService.AddOrUpdateTaxonAsync(plantInfo.Taxon);
                    Taxons.Add(taxon);
                }

                // Do we already have the same origin in our insert list?
                var originResult = newOrigins.FirstOrDefault(o => o.ParentOrigin.OriginId == Origin.OriginId
                                                                && o.ExternalId == plantInfo.Origin.ExternalId
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
                //newOrigins = (await _originService.AddOriginsAsync(newOrigins)).ToList();
                Origins.AddRange(newOrigins);
            }

            var distinctPlantInfos = plantInfos.DistinctBy(p => new
            {
                p.Taxon.Genus,
                p.Taxon.Species,
                p.Taxon.Subspecies,
                p.Taxon.Variety,
                p.Taxon.Subvariety,
                p.Taxon.Form
            });

            foreach (var plantInfo in distinctPlantInfos)
            {
                var origin = Origins.FirstOrDefault(o => o.ExternalId == plantInfo.Origin.ExternalId
                                                        && o.AltExternalId == plantInfo.Origin.AltExternalId);
                if (origin == null)
                {
                    origin = await _originService.GetOriginAsync(Origin.OriginId, plantInfo.Origin.ExternalId, plantInfo.Origin.AltExternalId);
                }

                plantInfo.Origin = origin;

                var plantInfoResult = await _plantInfoService.GetPlantInfoAsync(plantInfo.Origin.OriginId, plantInfo.Taxon.TaxonId);

                if (plantInfoResult == null)
                {
                    newPlantInfos.Add(plantInfo);
                }
            }

            if (newPlantInfos.Any())
            {
                //newPlantInfos = (await _plantInfoService.AddPlantInfosAsync(newPlantInfos)).ToList();
            }

            foreach (var newPlantInfo in newPlantInfos)
            {
                var plantInfo = plantInfos.First(p => p.Origin.OriginId == newPlantInfo.Origin.OriginId
                                                        && p.Taxon.TaxonId == newPlantInfo.Taxon.TaxonId);

                plantInfo.PlantInfoId = newPlantInfo.PlantInfoId;
                newPlantInfo.Taxon = plantInfo.Taxon;
                newPlantInfo.Origin = plantInfo.Origin;
                newPlantInfo.Lifeform = plantInfo.Lifeform;
            }

            var plantInfoLocations = plantInfos.Where(p => p.Locations != null && p.Locations.Any())
                                                .SelectMany(p => p.Locations)
                                                .DistinctBy(pl => new { pl.PlantInfo.PlantInfoId, pl.Location.LocationId })
                                                .ToList();

            if (plantInfoLocations.Any() && newPlantInfos.Any())
            {
                var states = plantInfos.SelectMany(p => p.Locations).Select(l => l.Location.StateOrProvince).Distinct();
                var regions = plantInfos.SelectMany(p => p.Locations).Select(l => l.Location.Region).Distinct();
                var countries = plantInfos.SelectMany(p => p.Locations).Select(l => l.Location.Country).Distinct();
                var locations = (await _locationService.GetLocationsAsync(l => countries.Contains(l.Country) || regions.Contains(l.Region))).ToList();

                var missingLocations = plantInfoLocations.GroupJoin(locations,
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

                var plantLocationsToAdd = new List<PlantLocation>();
                foreach (var plantInfoLocation in plantInfoLocations)
                {
                    var newPlantInfo = newPlantInfos.FirstOrDefault(npl => npl.Origin.OriginId == plantInfoLocation.PlantInfo.Origin.OriginId
                                                        && npl.Taxon.TaxonId == plantInfoLocation.PlantInfo.Taxon.TaxonId);
                    if (newPlantInfo != null)
                    {
                        var location = locations.First(l => l.Country == plantInfoLocation.Location.Country
                                                            && l.Region == plantInfoLocation.Location.Region);
                        plantLocationsToAdd.Add(new PlantLocation
                        {
                            PlantInfo = newPlantInfo,
                            Location = location,
                            Status = plantInfoLocation.Status
                        });
                    }
                }

                if (plantInfoLocations.Any())
                {
                    var plantLocationsResult = (await _plantInfoService.AddPlantLocations(plantLocationsToAdd)).ToList();

                    foreach (var newPlantInfo in newPlantInfos)
                    {
                        var newPlantInfoLocations = plantLocationsResult.Where(pl => pl.PlantInfo.PlantInfoId == newPlantInfo.PlantInfoId);
                        newPlantInfo.Locations = newPlantInfoLocations.Any() ? newPlantInfoLocations : null;
                    }
                }
            }

            return newPlantInfos;
        }
    }
}
