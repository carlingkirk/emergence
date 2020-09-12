namespace Emergence.Transform
{
    public interface IImportTransformOrchestrator
    {
        PlantInfoProcessor PlantInfoProcessor { get; }
        SynonymProcessor SynonymProcessor { get; }
    }
}
