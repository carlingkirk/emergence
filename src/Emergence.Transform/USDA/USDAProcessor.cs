using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Service.Interfaces;
using Models = Emergence.Data.Shared.Models;

namespace Emergence.Transform.USDA
{
    public class USDAProcessor : IUSDAProcessor
    {
        private readonly ILifeformService _lifeformService;
        private readonly IOriginService _originService;
        private readonly IPlantInfoService _plantInfoService;
        private readonly ITaxonService _taxonService;
        private Models.Origin Origin;
        private List<Models.Lifeform> Lifeforms { get; set; }
        private List<Models.Taxon> Taxons { get; set; }

        public USDAProcessor(ILifeformService lifeformService, IOriginService originService, IPlantInfoService plantInfoService, ITaxonService taxonService)
        {
            _lifeformService = lifeformService;
            _originService = originService;
            _plantInfoService = plantInfoService;
            _taxonService = taxonService;
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
    }
}
