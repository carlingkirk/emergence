using Emergence.Service.Interfaces;

namespace Emergence.Transform
{
    public class ImportTransformOrchestrator : IImportTransformOrchestrator
    {
        private readonly ILifeformService _lifeformService;
        private readonly IOriginService _originService;
        private readonly IPlantInfoService _plantInfoService;
        private readonly ILocationService _locationService;
        private readonly ITaxonService _taxonService;
        private readonly ISynonymService _synonymService;

        public ImportTransformOrchestrator(ILifeformService lifeformService, IOriginService originService, IPlantInfoService plantInfoService,
            ILocationService locationService, ITaxonService taxonService, ISynonymService synonymService)
        {
            _lifeformService = lifeformService;
            _originService = originService;
            _plantInfoService = plantInfoService;
            _locationService = locationService;
            _taxonService = taxonService;
            _synonymService = synonymService;
        }

        public PlantInfoProcessor GetPlantInfoProcessor => new PlantInfoProcessor(_lifeformService, _originService, _plantInfoService, _taxonService, _locationService);
        public SynonymProcessor GetSynonymProcessor => new SynonymProcessor(_synonymService, _originService, _taxonService);
    }
}
