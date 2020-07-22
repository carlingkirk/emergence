using System;
using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Extensions
{
    public static class SpecimenExtensions
    {
        public static Models.Specimen AsModel(this Specimen source) => new Models.Specimen
        {
            SpecimenId = source.Id,
            SpecimenStage = Enum.Parse<Models.SpecimenStage>(source.SpecimenStage),
            InventoryItem = source.InventoryItem != null ? source.InventoryItem.AsModel() : new Models.InventoryItem { InventoryItemId = source.InventoryItemId },
            Lifeform = source.Lifeform != null ? source.Lifeform.AsModel() : source.LifeformId.HasValue ? new Models.Lifeform { LifeformId = source.LifeformId.Value } : null,
            PlantInfo = source.PlantInfo != null ? source.PlantInfo.AsModel() : source.PlantInfoId.HasValue ? new Models.PlantInfo { PlantInfoId = source.PlantInfoId.Value } : null,
            DateCreated = source.DateCreated,
            DateModified = source.DateModified
        };

        public static Specimen AsStore(this Models.Specimen source) => new Specimen
        {
            Id = source.SpecimenId,
            SpecimenStage = source.SpecimenStage.ToString(),
            InventoryItemId = source.InventoryItem.InventoryItemId,
            LifeformId = source.Lifeform?.LifeformId,
            PlantInfoId = source.PlantInfo?.PlantInfoId,
            DateCreated = source.DateCreated ?? DateTime.UtcNow,
            DateModified = source.DateModified
        };
    }
}
