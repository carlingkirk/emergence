using System;
using System.ComponentModel.DataAnnotations;

namespace Emergence.Data.Shared.Stores
{
    public class Specimen : IIncludable<Specimen>, IIncludable<Specimen, InventoryItem>, IIncludable<Specimen, Lifeform>, IAuditable, IVisibile<Specimen>
    {
        public int Id { get; set; }
        public int InventoryItemId { get; set; }
        public int? LifeformId { get; set; }
        public string SpecimenStage { get; set; }
        public int? PlantInfoId { get; set; }
        public Lifeform Lifeform { get; set; }
        public InventoryItem InventoryItem { get; set; }
        public PlantInfo PlantInfo { get; set; }
        [StringLength(36)]
        public string CreatedBy { get; set; }
        [StringLength(36)]
        public string ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public Visibility Visibility => InventoryItem?.Visibility ?? Visibility.Hidden;
        public User User => InventoryItem?.User;
    }
}
