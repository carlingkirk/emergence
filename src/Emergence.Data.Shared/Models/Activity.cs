using System;

namespace Emergence.Data.Shared.Models
{
    public class Activity
    {
        public int ActivityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ActivityType ActivityType { get; set; }
        public Specimen Specimen { get; set; }
        public DateTime? DateOccured { get; set; }
        public DateTime? DateScheduled { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
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
