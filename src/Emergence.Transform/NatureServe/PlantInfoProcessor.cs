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
        public List<Origin> Origins { get; set; }
        public List<Lifeform> Lifeforms { get; set; }
        public List<Taxon> Taxons { get; set; }


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

        public PlantInfoProcessor(ILifeformService lifeformService, IOriginService originService, IPlantInfoService plantInfoService, ITaxonService taxonService,
            ILocationService locationService, List<Lifeform> lifeforms, List<Taxon> taxons, List<Origin> origins)
        {
            _lifeformService = lifeformService;
            _originService = originService;
            _plantInfoService = plantInfoService;
            _taxonService = taxonService;
            _locationService = locationService;

            Origins = origins;
            Lifeforms = lifeforms;
            Taxons = taxons;
        }

        public async Task InitializeOrigin(Origin origin)
        {
            Origin = await _originService.GetOriginAsync(origin.OriginId);
            if (Origin == null)
            {
                Origin = await _originService.AddOrUpdateOriginAsync(origin, null);
            }
        }

        public async Task InitializeLifeforms() => Lifeforms = await GetLifeforms();
        public async Task InitializeTaxons() => Taxons = await GetTaxons();
        public async Task InitializeOrigins() => Origins = await GetOrigins();

        public async Task<List<Lifeform>> GetLifeforms()
        {
            var lifeformResult = await _lifeformService.GetLifeformsAsync();
            Lifeforms = lifeformResult.ToList();
            return Lifeforms;
        }

        public async Task<List<Taxon>> GetTaxons()
        {
            var itisDate = new DateTime(2020, 08, 25);
            var taxonResult = await _taxonService.GetTaxonsAsync(t => t.DateCreated >= itisDate);
            return taxonResult.ToList();
        }

        public async Task<List<Origin>> GetOrigins()
        {
            var originResult = await _originService.GetOriginsOfParentAsync(Origin.OriginId);
            return originResult.ToList();
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

        public async Task<IEnumerable<PlantInfo>> Process(IEnumerable<PlantInfo> plantInfos)
        {
            var newOrigins = new List<Origin>();
            var newPlantInfos = new List<PlantInfo>();
            var newTaxons = new List<Taxon>();

            foreach (var plantInfo in plantInfos)
            {
                var lifeform = Lifeforms.FirstOrDefault(l => l.ScientificName == plantInfo.ScientificName);

                if (lifeform == null)
                {
                    lifeform = await _lifeformService.AddOrUpdateLifeformAsync(plantInfo.Lifeform);
                    Lifeforms.Add(lifeform);
                }

                plantInfo.Lifeform = lifeform;

                var taxon = Taxons.FirstOrDefault(t => t.Kingdom == plantInfo.Taxon.Kingdom
                                                && t.Genus == plantInfo.Taxon.Genus
                                                && t.Species == plantInfo.Taxon.Species
                                                && (t.Subspecies == plantInfo.Taxon.Subspecies || !string.IsNullOrEmpty(plantInfo.Taxon.Variety))
                                                && t.Variety == plantInfo.Taxon.Variety);

                if (taxon == null)
                {
                    // Do we already have the same taxon in our insert list?
                    var taxonResult = newTaxons.FirstOrDefault(t => t.Kingdom == plantInfo.Taxon.Kingdom
                                                && t.Genus == plantInfo.Taxon.Genus
                                                && t.Species == plantInfo.Taxon.Species
                                                && (t.Subspecies == plantInfo.Taxon.Subspecies || !string.IsNullOrEmpty(plantInfo.Taxon.Variety))
                                                && t.Variety == plantInfo.Taxon.Variety);

                    if (taxonResult == null)
                    {
                        newTaxons.Add(plantInfo.Taxon);
                    }
                }

                // Do we already have the same origin in our insert list?
                var originResult = newOrigins.FirstOrDefault(o => o.ParentOrigin.OriginId == Origin.OriginId
                                                                && o.ExternalId == plantInfo.Origin.ExternalId
                                                                && o.AltExternalId == plantInfo.Origin.AltExternalId);
                if (originResult == null)
                {
                    // See if it already exists, if not, add it to the insert list
                    originResult = Origins.FirstOrDefault(o => o.ExternalId == plantInfo.Origin.ExternalId && o.AltExternalId == plantInfo.Origin.AltExternalId);
                    if (originResult == null)
                    {
                        newOrigins.Add(plantInfo.Origin);
                    }
                }
            }

            if (newOrigins.Any())
            {
                newOrigins = (await _originService.AddOriginsAsync(newOrigins)).ToList();
                Origins.AddRange(newOrigins);
            }

            if (newTaxons.Any())
            {
                newTaxons = (await _taxonService.AddTaxonsAsync(newTaxons)).ToList();
                Taxons.AddRange(newTaxons);
            }

            foreach (var plantInfo in plantInfos)
            {
                var origin = Origins.FirstOrDefault(o => o.ExternalId == plantInfo.Origin.ExternalId
                                                      && o.AltExternalId == plantInfo.Origin.AltExternalId);
                if (origin == null)
                {
                    origin = await _originService.GetOriginAsync(Origin.OriginId, plantInfo.Origin.ExternalId, plantInfo.Origin.AltExternalId);
                }

                plantInfo.Origin = origin;

                if (plantInfo.Taxon != null)
                {
                    var taxon = Taxons.FirstOrDefault(t => t.Kingdom == plantInfo.Taxon.Kingdom
                                                        && t.Genus == plantInfo.Taxon.Genus
                                                        && t.Species == plantInfo.Taxon.Species
                                                        && (t.Subspecies == plantInfo.Taxon.Subspecies || !string.IsNullOrEmpty(plantInfo.Taxon.Variety))
                                                        && t.Variety == plantInfo.Taxon.Variety);
                    if (taxon == null)
                    {
                        Console.WriteLine("wtf?");
                    }

                    plantInfo.Taxon = taxon;
                }

                newPlantInfos.Add(plantInfo);
            }

            if (newPlantInfos.Any())
            {
                newPlantInfos = (await _plantInfoService.AddPlantInfosAsync(newPlantInfos)).ToList();
            }

            foreach (var newPlantInfo in newPlantInfos)
            {
                var plantInfo = plantInfos.First(p => p.Origin.OriginId == newPlantInfo.Origin.OriginId);

                plantInfo.PlantInfoId = newPlantInfo.PlantInfoId;
                newPlantInfo.Taxon = plantInfo.Taxon;
                newPlantInfo.Origin = plantInfo.Origin;
                newPlantInfo.Lifeform = plantInfo.Lifeform;
            }

            var plantInfoLocations = plantInfos.Where(p => p.Locations != null && p.Locations.Any())
                                                .SelectMany(p => p.Locations)
                                                .DistinctBy(pl => new { pl.PlantInfo.Origin.AltExternalId, pl.Location.StateOrProvince })
                                                .ToList();

            if (plantInfoLocations.Any() && newPlantInfos.Any())
            {
                var states = plantInfos.SelectMany(p => p.Locations).Select(l => l.Location.StateOrProvince).Distinct();
                var countries = plantInfos.SelectMany(p => p.Locations).Select(l => l.Location.Country).Distinct();
                var locations = (await _locationService.GetLocationsAsync(l =>
                    (countries.Contains(l.Country) || states.Contains(l.StateOrProvince)) &&
                    l.PostalCode == null && l.Region == null && l.AddressLine1 == null & l.City == null)).ToList();

                var missingLocations = plantInfoLocations.Select(pl => pl.Location)
                    .GroupJoin(locations,
                        pl => new { pl.StateOrProvince, pl.Country },
                        l => new { l.StateOrProvince, l.Country },
                        (pl, l) => new { pl, l })
                    .Where(pll => !pll.l.Any())
                    .Select(pll => pll.pl).DistinctBy(pl => new { pl.StateOrProvince, pl.Country })
                    .ToList();

                if (missingLocations.Any())
                {
                    var locationResult = await _locationService.AddLocationsAsync(missingLocations);
                    locations.AddRange(locationResult.ToList());
                }

                var plantLocationsToAdd = new List<PlantLocation>();
                foreach (var plantInfoLocation in plantInfoLocations)
                {
                    var newPlantInfo = newPlantInfos.FirstOrDefault(npl => npl.Origin.AltExternalId == plantInfoLocation.PlantInfo.Origin.AltExternalId);
                    if (newPlantInfo != null)
                    {
                        var location = locations.First(l => l.Country == plantInfoLocation.Location.Country
                                                         && l.StateOrProvince == plantInfoLocation.Location.StateOrProvince);

                        plantLocationsToAdd.Add(new PlantLocation
                        {
                            PlantInfo = newPlantInfo,
                            Location = location,
                            Status = plantInfoLocation.Status,
                            ConservationStatus = plantInfoLocation.ConservationStatus
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
