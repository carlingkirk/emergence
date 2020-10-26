using System;
using System.ComponentModel.DataAnnotations;

namespace Emergence.Data.Shared.Stores
{
    public class PlantInfo : IIncludable<PlantInfo>, IIncludable<PlantInfo, Origin>, IIncludable<PlantInfo, Lifeform>, IIncludable<PlantInfo, Taxon>, IAuditable, IVisibile<InventoryItem>
    {
        public int Id { get; set; }
        public int LifeformId { get; set; }
        public Lifeform Lifeform { get; set; }
        public int? OriginId { get; set; }
        public Origin Origin { get; set; }
        public int? TaxonId { get; set; }
        public Taxon Taxon { get; set; }
        [StringLength(100)]
        public string ScientificName { get; set; }
        [StringLength(100)]
        public string CommonName { get; set; }
        public bool? Preferred { get; set; }
        [StringLength(36)]
        public short? MinimumBloomTime { get; set; }
        [StringLength(36)]
        public short? MaximumBloomTime { get; set; }
        [StringLength(36)]
        public double? MinimumHeight { get; set; }
        [StringLength(36)]
        public double? MaximumHeight { get; set; }
        [StringLength(36)]
        public string HeightUnit { get; set; }
        [StringLength(36)]
        public double? MinimumSpread { get; set; }
        [StringLength(36)]
        public double? MaximumSpread { get; set; }
        [StringLength(36)]
        public string SpreadUnit { get; set; }
        [StringLength(36)]
        public string MinimumWater { get; set; }
        [StringLength(36)]
        public string MaximumWater { get; set; }
        [StringLength(36)]
        public string MinimumLight { get; set; }
        [StringLength(36)]
        public string MaximumLight { get; set; }
        public string StratificationStages { get; set; }
        [StringLength(8)]
        public string MinimumZone { get; set; }
        [StringLength(8)]
        public string MaximumZone { get; set; }
        public Visibility Visibility { get; set; }
        [StringLength(36)]
        public string CreatedBy { get; set; }
        [StringLength(36)]
        public string ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public User User { get; set; }
    }
}
