using Emergence.Data;
using Emergence.Data.Shared.Stores;
using Emergence.Service.Interfaces;
using Emergence.Service.Search;
using SearchModels = Emergence.Data.Shared.Search.Models;

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
        private readonly IRepository<PlantInfo> _plantInfoRepository;
        private readonly ISearchClient<SearchModels.PlantInfo> _searchClient;
        private readonly IIndex<SearchModels.PlantInfo> _plantInfoIndex;

        public ImportTransformOrchestrator(ILifeformService lifeformService, IOriginService originService, IPlantInfoService plantInfoService,
            ILocationService locationService, ITaxonService taxonService, ISynonymService synonymService, IRepository<PlantInfo> plantInfoRepository,
            ISearchClient<SearchModels.PlantInfo> searchClient, IIndex<SearchModels.PlantInfo> plantInfoIndex)
        {
            _lifeformService = lifeformService;
            _originService = originService;
            _plantInfoService = plantInfoService;
            _locationService = locationService;
            _taxonService = taxonService;
            _synonymService = synonymService;
            _plantInfoRepository = plantInfoRepository;
            _searchClient = searchClient;
            _plantInfoIndex = plantInfoIndex;
        }

        public PlantInfoProcessor GetPlantInfoProcessor => new PlantInfoProcessor(_lifeformService, _originService, _plantInfoService, _taxonService, _locationService);
        public SynonymProcessor GetSynonymProcessor => new SynonymProcessor(_synonymService, _originService, _taxonService);
        public ElasticProcessor GetElasticProcessor => new ElasticProcessor(_plantInfoRepository, _plantInfoIndex);
    }
}
