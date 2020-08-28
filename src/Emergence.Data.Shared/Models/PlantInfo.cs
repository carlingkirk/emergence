using System;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Interfaces;

namespace Emergence.Data.Shared.Models
{
    public class PlantInfo : ILifeform
    {
        public int PlantInfoId { get; set; }
        public int LifeformId { get; set; }
        public string ScientificName { get; set; }
        public string CommonName { get; set; }
        public bool? Preferred { get; set; }
        public BloomTime BloomTime { get; set; }
        public Height Height { get; set; }
        public Spread Spread { get; set; }
        public Requirements Requirements { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public Taxon Taxon { get; set; }
        public Origin Origin { get; set; }
        public Lifeform Lifeform { get; set; }
    }

    public class BloomTime
    {
        public Month MinimumBloomTime { get; set; }
        public Month MaximumBloomTime { get; set; }

        public string ToFriendlyString()
        {
            if (MinimumBloomTime != Month.Unknown && MaximumBloomTime != Month.Unknown)
            {
                return MinimumBloomTime.ToFriendlyName() + " - " + MaximumBloomTime.ToFriendlyName();
            }
            else if (MinimumBloomTime != Month.Unknown)
            {
                return MinimumBloomTime.ToFriendlyName();
            }
            else if (MaximumBloomTime != Month.Unknown)
            {
                return MaximumBloomTime.ToFriendlyName();
            }
            return null;
        }
    }

    public class Height
    {
        public double? MinimumHeight { get; set; }
        public double? MaximumHeight { get; set; }
        public DistanceUnit Unit { get; set; }

        public string ToFriendlyString()
        {
            string height = null;
            if (MinimumHeight != null && MaximumHeight != null)
            {
                height = string.Format("{0:0.00}", MinimumHeight) + " - " + string.Format("{0:0.00}", MaximumHeight);
            }
            else if (MinimumHeight != null)
            {
                height = string.Format("{0:0.00}", MinimumHeight);
            }
            else if (MaximumHeight != null)
            {
                height = string.Format("{0:0.00}", MaximumHeight);
            }

            if (Unit != DistanceUnit.Unknown)
            {
                height = height + " " + Unit.ToFriendlyName();
            }

            return height;
        }
    }

    public class Spread
    {
        public double? MinimumSpread { get; set; }
        public double? MaximumSpread { get; set; }
        public DistanceUnit Unit { get; set; }

        public string ToFriendlyString()
        {
            string height = null;
            if (MinimumSpread != null && MaximumSpread != null)
            {
                height = string.Format("{0:0.00}", MinimumSpread) + " - " + string.Format("{0:0.00}", MaximumSpread);
            }
            else if (MinimumSpread != null)
            {
                height = string.Format("{0:0.00}", MinimumSpread);
            }
            else if (MaximumSpread != null)
            {
                height = string.Format("{0:0.00}", MaximumSpread);
            }

            if (Unit != DistanceUnit.Unknown)
            {
                height = height + " " + Unit.ToFriendlyName();
            }

            return height;
        }
    }
}
