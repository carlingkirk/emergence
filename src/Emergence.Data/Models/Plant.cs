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
        public string BloomTime { get; set; }
        public string Zones { get; set; }
        public string Height { get; set; }
        public string Spread { get; set; }
        public Requirements Requirements { get; set; }
    }

    public class Requirements
    {
        public WaterRequirements WaterRequirements { get; set; }
        public LightRequirements LightRequirements { get; set; }
        public IEnumerable<SoilTypes> SoilRequirements { get; set; }
        public StratificationRequirements StratificationRequirements { get; set; }
        public string ScarificationRequirements { get; set; }
        public string SeedStorageRequirements { get; set; }
    }

    public class WaterRequirements
    {
        public WaterTypes MinimumWater { get; set; }
        public WaterTypes MaximumWater { get; set; }
    }

    public class LightRequirements
    {
        public LightTypes MinimumLight { get; set; }
        public LightTypes MaximumLight { get; set; }
    }

    public class StratificationRequirements
    {
        public IDictionary<int, StratificationStage> StratificationStages { get; set; }
    }

    public class StratificationStage
    {
        public short DayLength { get; set; }
        public short MinimumTemperature { get; set; }
        public short MaximumTemperature { get; set; }
    }

    public enum WaterTypes
    {
        Wet,
        MediumWet,
        Medium,
        MediumDry,
        Dry
    }

    public enum LightTypes
    {
        FullSun,
        PartSun,
        PartShade,
        FullShade
    }

    public enum SoilTypes
    {
        Fertile,
        Loamy,
        Rocky,
        Clay,
        Peaty,
        Swamp,
        Water
    }
}
