using Emergence.Data.Shared.Interfaces;
using Emergence.Data.Shared.Models;
using Emergence.Data.Shared.Search;

namespace Emergence.Data.Shared
{
    public class FindParams : IFindParams
    {
        public FindParams()
        {
            Skip = 0;
            Take = 10;
            SortBy = null;
            SortDirection = SortDirection.Ascending;
        }

        public string SearchText { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public string SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
        public string CreatedBy { get; set; }
        public bool ContactsOnly { get; set; }
        public string SearchTextQuery => SearchText != null ? "%" + SearchText + "%" : null;
    }

    public class FindParams<T> : IFindParams
    {
        public FindParams()
        {
            Skip = 0;
            Take = 10;
            SortBy = null;
            SortDirection = SortDirection.Ascending;
        }
        public T Shape { get; set; }

        public bool UseNGrams { get; set; }
        public string SearchText { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public string SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
        public string CreatedBy { get; set; }
    }

    public class PlantInfoFindParams : FindParams<PlantInfo>, IFindParams
    {
        public Lifeform Lifeform { get; set; }

        public PlantInfoFindParams()
        {
            Skip = 0;
            Take = 10;
            SortBy = null;
            SortDirection = SortDirection.Ascending;
        }

        public PlantInfoFilters Filters { get; set; }

        public static Filter GetFilter(string key, PlantInfoFindParams plantInfoFindParams)
        {
            switch (key)
            {
                case "Zone":
                    return plantInfoFindParams.Filters.ZoneFilter;
                case "Region":
                    return plantInfoFindParams.Filters.RegionFilter;
                case "Bloom":
                    return plantInfoFindParams.Filters.BloomFilter;
                case "Water":
                    return plantInfoFindParams.Filters.WaterFilter;
                case "Light":
                    return plantInfoFindParams.Filters.LightFilter;
                case "MinSpread":
                    return plantInfoFindParams.Filters.SpreadFilter;
                case "MaxSpread":
                    return plantInfoFindParams.Filters.SpreadFilter;
                case "MinHeight":
                    return plantInfoFindParams.Filters.HeightFilter;
                case "MaxHeight":
                    return plantInfoFindParams.Filters.HeightFilter;
                default:
                    return null;
            }
        }
    }

    public class SpecimenFindParams : FindParams<Specimen>, IFindParams
    {
        public SpecimenFindParams()
        {
            Skip = 0;
            Take = 10;
            SortBy = null;
            SortDirection = SortDirection.Ascending;
        }

        public SpecimenFilters Filters { get; set; }

        public static Filter GetFilter(string key, SpecimenFindParams findParams)
        {
            switch (key)
            {
                case "Stage":
                    return findParams.Filters.StageFilter;
                default:
                    return null;
            }
        }
    }
}
