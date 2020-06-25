using Emergence.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Emergence.Data.Models
{
    public class Plant : ILifeform
    {
        public long LifeformId { get; set; }
        public Taxon Taxon { get; set; }
        public string ScientificName { get; set; }
        public string CommonName { get; set; }
        public BloomTime BloomTime { get; set; }
        public Height Height { get; set; }
        public Spread Spread { get; set; }
        public Requirements Requirements { get; set; }
    }

    public class BloomTime
    {
        public Month MinimumBloomTime { get; set; }
        public Month MaximumBloomTime { get; set; }
    }

    public class Height
    {
        public double MinimumHeight { get; set; }
        public double MaximumHeight { get; set; }
        public Unit Unit { get; set; }
    }

    public class Spread
    {
        public double MinimumSpread { get; set; }
        public double MaximumSpread { get; set; }
        public Unit Unit { get; set; }
    }

    public enum Month
    {
        Jan = 1,
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

    public enum Unit
    {
        Inches,
        Feet,
        Centimeters,
        Meters
    }
}
