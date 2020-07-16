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
            InventoryItem = source.InventoryItem?.AsModel(),
            Lifeform = source.Lifeform?.AsModel(),
            PlantInfo = source.PlantInfo?.AsModel(),
            DateCreated = source.DateCreated,
            DateModified = source.DateModified
        };

        public static Specimen AsStore(this Models.Specimen source) => new Specimen
        {
            Id = source.SpecimenId,
            SpecimenStage = source.SpecimenStage.ToString(),
            InventoryItemId = source.InventoryItem.InventoryItemId,
            LifeformId = source.Lifeform.LifeformId,
            PlantInfoId = source.PlantInfo?.PlantInfoId,
            DateCreated = source.DateCreated,
            DateModified = source.DateModified
        };
    }
}
