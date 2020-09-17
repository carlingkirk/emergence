using System.ComponentModel;

namespace Emergence.Data.Shared
{
    public enum ActivityType
    {
        [Description("Germinate")]
        Germination,
        [Description("Stratify")]
        Stratification,
        [Description("Divide")]
        Division,
        [Description("Take cutting")]
        Cutting,
        [Description("Collect seeds")]
        SeedCollection,
        [Description("Progress check")]
        ProgressCheck,
        [Description("Plant in ground")]
        PlantInGround,
        [Description("Repot")]
        Repotting,
        [Description("Water")]
        Watering,
        [Description("Fertilize")]
        Fertilization,
        Custom,
        [Description("Add to wishlist")]
        AddToWishlist,
        Purchase
    }
}
