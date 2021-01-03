using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Emergence.Data.Shared.Search;
using Emergence.Test.Data.Fakes.Stores;
using FluentAssertions;
using Xunit;

namespace Emergence.Test.Emergence.Data.Shared.Search
{
    public class TestHeightFilter
    {
        [Fact]
        public void TestSerialization()
        {
            var filters = new List<Filter>
            {
                new HeightFilter
                {
                    MinimumValue = 1,
                    MaximumValue = 2
                },
                new RegionFilter
                {
                    Value = "North America"
                }
            };
            ConverterInfo.BuildConverterInfo(Assembly.GetExecutingAssembly());
            var jsonFilters = FilterSerializer.Serialize(filters);

            filters = FilterSerializer.Deserialize<List<Filter>>(jsonFilters);

            var heightFilter = new HeightFilter((RangeFilter<double?>)filters.First(f => f.Name == "Height"));

            heightFilter.Should().NotBeNull();
            heightFilter.FilterType.Should().Be(FilterType.Double);
            heightFilter.InputType.Should().Be(InputType.SelectRange);
            heightFilter.MinimumValue.Should().Be(1);
            heightFilter.MaximumValue.Should().Be(2);

            var plantInfos = FakePlantInfos.Get().AsQueryable();
            var filteredPlantInfos = plantInfos.Where(heightFilter.Filter).ToList();

            filteredPlantInfos.Count.Should().Be(2);
        }
    }
}
