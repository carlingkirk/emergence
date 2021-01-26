namespace Emergence.Transform
{
    public interface IImportTransformOrchestrator
    {
        PlantInfoProcessor GetPlantInfoProcessor { get; }
        SynonymProcessor GetSynonymProcessor { get; }
        ElasticPlantInfoProcessor GetElasticPlantInfoProcessor { get; }
        ElasticSpecimenProcessor GetElasticSpecimenProcessor { get; }
    }
}
