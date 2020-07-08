using System;
using System.ComponentModel;
using Emergence.Data.Shared.Interfaces;

namespace Emergence.Data.Shared.Models
{
    public class PlantInfo : ILifeform
    {
        public int PlantInfoId { get; set; }
        public int LifeformId { get; set; }
        public Taxon Taxon { get; set; }
        public string ScientificName { get; set; }
        public string CommonName { get; set; }
        public BloomTime BloomTime { get; set; }
        public Height Height { get; set; }
        public Spread Spread { get; set; }
        public Requirements Requirements { get; set; }
        public Origin Origin { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }

    public class BloomTime
    {
        public Month MinimumBloomTime { get; set; }
        public Month MaximumBloomTime { get; set; }
    }

    public class Height
    {
        public double? MinimumHeight { get; set; }
        public double? MaximumHeight { get; set; }
        public DistanceUnit Unit { get; set; }
    }

    public class Spread
    {
        public double? MinimumSpread { get; set; }
        public double? MaximumSpread { get; set; }
        public DistanceUnit Unit { get; set; }
    }

    public enum Month
    {
        [Description("")]
        Unknown,
        Jan,
        Feb,
        Mar,
        Apr,
        May,
        Jun,
        Jul,
        Aug,
        Sep,
        Oct,
        Nov,
        Dec
    }

    public enum DistanceUnit
    {
        [Description("ft")]
        Feet,
        [Description("in")]
        Inches,
        [Description("m")]
        Meters,
        [Description("cm")]
        Centimeters
    }
}
