using System.ComponentModel;

namespace Emergence.Data.Shared
{
    public enum PhotoType
    {
        Activity,
        Specimen,
        [Description("Inventory Item")]
        InventoryItem,
        Origin,
        [Description("Plant Profile")]
        PlantInfo,
        User
    }
}
