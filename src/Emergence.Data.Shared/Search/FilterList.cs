using Emergence.Data.Shared.Models;

namespace Emergence.Data.Shared.Search
{
    public interface IFilterList<T>
    {
    }

    public class PlantInfoFilters : IFilterList<PlantInfo>
    {
        public PlantInfoFilters()
        {
            ZoneFilter = new ZoneFilter();
            RegionFilter = new RegionFilter();
            HeightFilter = new HeightFilter();
            SpreadFilter = new SpreadFilter();
            LightFilter = new LightFilter();
            WaterFilter = new WaterFilter();
            BloomFilter = new BloomFilter();
            NativeFilter = new NativeFilter();
        }

        public ZoneFilter ZoneFilter { get; set; }
        public RegionFilter RegionFilter { get; set; }
        public HeightFilter HeightFilter { get; set; }
        public SpreadFilter SpreadFilter { get; set; }
        public LightFilter LightFilter { get; set; }
        public WaterFilter WaterFilter { get; set; }
        public BloomFilter BloomFilter { get; set; }
        public NativeFilter NativeFilter { get; set; }
    }

    public class SpecimenFilters : IFilterList<Specimen>
    {
        public SpecimenFilters()
        {
            StageFilter = new StageFilter();
        }

        public StageFilter StageFilter { get; set; }
    }
}
