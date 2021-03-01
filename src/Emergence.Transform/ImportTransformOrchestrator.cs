using System.Collections.Generic;
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
        private readonly IRepository<Specimen> _specimenRepository;
        private readonly ISearchClient<SearchModels.PlantInfo> _searchClient;
        private readonly IIndex<SearchModels.PlantInfo, Emergence.Data.Shared.Models.PlantInfo> _plantInfoIndex;
        private readonly IIndex<SearchModels.Specimen, Emergence.Data.Shared.Models.Specimen> _specimenIndex;

        public ImportTransformOrchestrator(ILifeformService lifeformService, IOriginService originService, IPlantInfoService plantInfoService,
            ILocationService locationService, ITaxonService taxonService, ISynonymService synonymService, IRepository<PlantInfo> plantInfoRepository, IRepository<Specimen> specimenRepository,
            ISearchClient<SearchModels.PlantInfo> searchClient, IIndex<SearchModels.PlantInfo, Emergence.Data.Shared.Models.PlantInfo> plantInfoIndex,
            IIndex<SearchModels.Specimen, Emergence.Data.Shared.Models.Specimen> specimenIndex)
        {
            _lifeformService = lifeformService;
            _originService = originService;
            _plantInfoService = plantInfoService;
            _locationService = locationService;
            _taxonService = taxonService;
            _synonymService = synonymService;
            _plantInfoRepository = plantInfoRepository;
            _specimenRepository = specimenRepository;
            _searchClient = searchClient;
            _plantInfoIndex = plantInfoIndex;
            _specimenIndex = specimenIndex;
        }

        public NatureServe.PlantInfoProcessor GetNatureServePlantInfoProcessor()
            => new NatureServe.PlantInfoProcessor(_lifeformService, _originService, _plantInfoService, _taxonService, _locationService, _plantInfoIndex);
        public NatureServe.PlantInfoProcessor GetNatureServePlantInfoProcessor(List<Emergence.Data.Shared.Models.Lifeform> lifeforms, List<Emergence.Data.Shared.Models.Taxon> taxons,
            List<Emergence.Data.Shared.Models.Origin> origins)
            => new NatureServe.PlantInfoProcessor(_lifeformService, _originService, _plantInfoService, _taxonService, _locationService, _plantInfoIndex, lifeforms, taxons, origins);
        public PlantInfoProcessor GetPlantInfoProcessor => new PlantInfoProcessor(_lifeformService, _originService, _plantInfoService, _taxonService, _locationService);
        public SynonymProcessor GetSynonymProcessor => new SynonymProcessor(_synonymService, _originService, _taxonService);
        public ElasticPlantInfoProcessor GetElasticPlantInfoProcessor => new ElasticPlantInfoProcessor(_plantInfoRepository, _plantInfoIndex);
        public ElasticSpecimenProcessor GetElasticSpecimenProcessor => new ElasticSpecimenProcessor(_specimenRepository, _specimenIndex);
    }
}
