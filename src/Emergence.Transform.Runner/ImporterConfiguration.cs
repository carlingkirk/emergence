namespace Emergence.Transform.Runner
{
    public class ImporterConfiguration
    {
        public string Name { get; set; }
        public ImporterType Type { get; set; }
        public string Filename { get; set; }
        public bool HasHeaders { get; set; }
    }

    public enum ImporterType
    {
        TextImporter
    }
}
