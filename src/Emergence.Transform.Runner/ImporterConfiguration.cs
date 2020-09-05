namespace Emergence.Transform.Runner
{
    public class ImporterConfiguration
    {
        public string Name { get; set; }
        public ImporterType Type { get; set; }
        public string Filename { get; set; }
        public string ConnectionString { get; set; }
        public string SqlQuery { get; set; }
        public bool HasHeaders { get; set; }
        public bool IsActive { get; set; }
    }

    public enum ImporterType
    {
        TextImporter,
        SqlImporter
    }
}
