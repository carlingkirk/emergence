using System;

namespace Emergence.Data.Shared.Stores
{
    public class PlantInfo
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
        public long OriginId { get; set; }
        public long TaxonId { get; set; }
        public string ScientificName { get; set; }
        public string CommonName { get; set; }
        public bool? Preferred { get; set; }
        public short MinimumBloomTime { get; set; }
        public short MaximumBloomTime { get; set; }
        public double? MinimumHeight { get; set; }
        public double? MaximumHeight { get; set; }
        public string HeightUnit { get; set; }
        public double? MinimumSpread { get; set; }
        public double? MaximumSpread { get; set; }
        public string SpreadUnit { get; set; }
        public string MinimumWater { get; set; }
        public string MaximumWater { get; set; }
        public string MinimumLight { get; set; }
        public string MaximumLight { get; set; }
        public bool? SeedRefrigerate { get; set; }
        public string StratificationStage1 { get; set; }
        public string StratificationStage2 { get; set; }
        public string StratificationStage3 { get; set; }
        public string ScarificationType1 { get; set; }
        public string ScarificationType2 { get; set; }
        public string ScarificationType3 { get; set; }
        public string MinimumZone { get; set; }
        public string MaximumZone { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
