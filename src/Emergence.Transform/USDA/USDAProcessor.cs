using System.Threading.Tasks;
using Emergence.Data.Shared.Stores;
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

        public async Task<Models.PlantInfo> Process(Models.PlantInfo plantInfo)
        {
            var lifeformResult = await _lifeformService.GetLifeformByScientificNameAsync(plantInfo.ScientificName);
            if (lifeformResult == null)
            {
                lifeformResult = await _lifeformService.AddOrUpdateLifeformAsync(plantInfo.Lifeform);
            }

            var originResult = await _originService.GetOriginAsync(Origin.OriginId, plantInfo.Origin.ExternalId, plantInfo.Origin.AltExternalId);
            if (originResult == null)
            {
                originResult = await _originService.AddOrUpdateOriginAsync(plantInfo.Origin, null);
            }

            var taxonResult = await _taxonService.GetTaxonAsync(plantInfo.Taxon.Genus, plantInfo.Taxon.Species, plantInfo.Taxon.Subspecies,
                plantInfo.Taxon.Variety, plantInfo.Taxon.Subvariety, plantInfo.Taxon.Form);
            if (taxonResult == null)
            {
                taxonResult = await _taxonService.AddOrUpdateTaxonAsync(plantInfo.Taxon);
            }

            var plantInfoResult = await _plantInfoService.GetPlantInfoAsync(originResult.OriginId, taxonResult.TaxonId);

            if (plantInfoResult == null)
            {
                plantInfo.Origin = originResult;
                plantInfo.Lifeform = lifeformResult;
                plantInfo.Taxon = taxonResult;
                plantInfoResult = await _plantInfoService.AddOrUpdatePlantInfoAsync(plantInfo);
            }

            return plantInfoResult;
        }
    }
}
