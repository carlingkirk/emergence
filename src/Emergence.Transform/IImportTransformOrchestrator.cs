using System.Collections.Generic;

namespace Emergence.Transform
{
    public interface IImportTransformOrchestrator
    {
        public NatureServe.PlantInfoProcessor GetNatureServePlantInfoProcessor();
        public NatureServe.PlantInfoProcessor GetNatureServePlantInfoProcessor(List<Emergence.Data.Shared.Models.Lifeform> lifeforms, List<Emergence.Data.Shared.Models.Taxon> taxons, List<Emergence.Data.Shared.Models.Origin> origins);
        PlantInfoProcessor GetPlantInfoProcessor { get; }
        SynonymProcessor GetSynonymProcessor { get; }
        ElasticPlantInfoProcessor GetElasticPlantInfoProcessor { get; }
        ElasticSpecimenProcessor GetElasticSpecimenProcessor { get; }
    }
}
