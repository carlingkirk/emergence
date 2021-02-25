namespace Emergence.Transform
{
    public interface IImportTransformOrchestrator
    {
        PlantInfoProcessor GetPlantInfoProcessor { get; }
        NatureServe.PlantInfoProcessor GetNatureServePlantInfoProcessor { get; }
        SynonymProcessor GetSynonymProcessor { get; }
        ElasticPlantInfoProcessor GetElasticPlantInfoProcessor { get; }
        ElasticSpecimenProcessor GetElasticSpecimenProcessor { get; }
    }
}
