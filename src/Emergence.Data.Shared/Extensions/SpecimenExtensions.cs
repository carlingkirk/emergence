using System;
using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Extensions
{
    public static class SpecimenExtensions
    {
        public static Models.Specimen AsModel(this Specimen source) => new Models.Specimen
        {
            SpecimenId = source.Id,
            SpecimenStage = Enum.Parse<SpecimenStage>(source.SpecimenStage),
            InventoryItem = source.InventoryItem != null ? source.InventoryItem.AsModel() : new Models.InventoryItem { InventoryItemId = source.InventoryItemId },
            Name = source.InventoryItem?.Name,
            Lifeform = source.Lifeform != null ? source.Lifeform.AsModel() : source.LifeformId.HasValue ? new Models.Lifeform { LifeformId = source.LifeformId.Value } : null,
            PlantInfo = source.PlantInfo != null ? source.PlantInfo.AsModel() : source.PlantInfoId.HasValue ? new Models.PlantInfo { PlantInfoId = source.PlantInfoId.Value } : null,
            ParentSpecimen = source.ParentSpecimen != null ?
                source.ParentSpecimen.AsModel() : source.ParentSpecimenId.HasValue ?
                    new Models.Specimen { SpecimenId = source.ParentSpecimenId.Value } : null,
            Quantity = source.InventoryItem.Quantity,
            Notes = source.Notes,
            CreatedBy = source.CreatedBy,
            ModifiedBy = source.ModifiedBy,
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
            ParentSpecimenId = source.ParentSpecimen?.SpecimenId,
            Notes = source.Notes,
            CreatedBy = source.CreatedBy,
            ModifiedBy = source.ModifiedBy,
            DateCreated = source.DateCreated ?? DateTime.UtcNow,
            DateModified = source.DateModified
        };

        public static Search.Models.Specimen AsSearchModel(this Specimen source) =>
            new Search.Models.Specimen
            {
                Id = source.Id,
                SpecimenStage = Enum.Parse<SpecimenStage>(source.SpecimenStage),
                InventoryItem = source.InventoryItem?.AsSearchModel(),
                Lifeform = source.Lifeform?.AsSearchModel(),
                ParentSpecimen = source.ParentSpecimen?.AsSearchModel(),
                Name = source.InventoryItem.Name,
                Quantity = source.InventoryItem.Quantity,
                Notes = source.Notes,
                CreatedBy = source.CreatedBy,
                ModifiedBy = source.ModifiedBy,
                DateCreated = source.DateCreated,
                DateModified = source.DateModified
            };
    }
}
