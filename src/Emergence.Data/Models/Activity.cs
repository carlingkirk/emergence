namespace Emergence.Data.Models
{
    public class Activity
    {
        public long ActivityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ActivityType ActivityType { get; set; }
    }

    public enum ActivityType
    {
        Order,
        Stratification,
        Germination,
        Division,
        Cutting,
        SeedCollection,
        ProgressCheck,
        PlantInGround,
        Repotting,
        Watering,
        Fertilization
    }
}
