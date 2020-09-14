namespace Emergence.Transform
{
    public interface IImportTransformOrchestrator
    {
        PlantInfoProcessor GetPlantInfoProcessor { get; }
        SynonymProcessor GetSynonymProcessor { get; }
    }
}
