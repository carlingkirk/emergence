namespace Emergence.Data.Shared.Stores
{
    public class Activity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ActivityType { get; set; }
        public int? SpecimenId { get; set; }
    }
}
