using System;
using System.Collections.Generic;

namespace Emergence.Data.Shared.Search.Models
{
    public class PlantInfo
    {
        public int Id { get; set; }
        public string ScientificName { get; set; }
        public string CommonName { get; set; }
        public bool? Preferred { get; set; }
        public short? MinimumBloomTime { get; set; }
        public short? MaximumBloomTime { get; set; }
        public IEnumerable<Month> BloomTimes { get; set; }
        public double? MinimumHeight { get; set; }
        public double? MaximumHeight { get; set; }
        public DistanceUnit? HeightUnit { get; set; }
        public double? MinimumSpread { get; set; }
        public double? MaximumSpread { get; set; }
        public DistanceUnit? SpreadUnit { get; set; }
        public WaterType? MinimumWater { get; set; }
        public WaterType? MaximumWater { get; set; }
        public IEnumerable<WaterType> WaterTypes { get; set; }
        public LightType? MinimumLight { get; set; }
        public LightType? MaximumLight { get; set; }
        public IEnumerable<LightType> LightTypes { get; set; }
        public IEnumerable<StratificationStage> StratificationStages { get; set; }
        public Zone MinimumZone { get; set; }
        public Zone MaximumZone { get; set; }
        public IEnumerable<Zone> Zones { get; set; }
        public IEnumerable<Shared.Models.WildlifeEffect> WildlifeEffects { get; set; }
        public string Notes { get; set; }
        public Visibility Visibility { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public Lifeform Lifeform { get; set; }
        public Origin Origin { get; set; }
        public Taxon Taxon { get; set; }
        public User User { get; set; }
        public IEnumerable<PlantLocation> PlantLocations { get; set; }
        public IEnumerable<Synonym> Synonyms { get; set; }
    }
}
