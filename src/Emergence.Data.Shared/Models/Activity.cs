using System;
using System.ComponentModel;

namespace Emergence.Data.Shared.Models
{
    public class Activity
    {
        public int ActivityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ActivityType ActivityType { get; set; }
        public string CustomActivityType { get; set; }

        public string UserId { get; set; }
        public DateTime? DateOccured { get; set; }
        public DateTime? DateScheduled { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public Specimen Specimen { get; set; }
        public Photo Photo { get; set; }
    }

    public enum ActivityType
    {
        Custom,
        [Description("Add to wishlist")]
        Wishlist,
        Purchase,
        [Description("Stratify")]
        Stratification,
        [Description("Germinate")]
        Germination,
        [Description("Divide")]
        Division,
        [Description("Take cutting")]
        Cutting,
        [Description("Collect seed")]
        SeedCollection,
        [Description("Check progress")]
        ProgressCheck,
        [Description("Plant in ground")]
        PlantInGround,
        [Description("Repot")]
        Repotting,
        [Description("Water")]
        Watering,
        [Description("Fertilize")]
        Fertilization
    }
}
