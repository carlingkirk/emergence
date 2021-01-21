using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Emergence.Data.Shared.Search;
using Emergence.Test.Data.Fakes.Stores;
using FluentAssertions;
using Xunit;

namespace Emergence.Test.Emergence.Data.Shared.Search
{
    public class TestRegionFilter
    {
        [Fact]
        public void TestSerialization()
        {
            var filters = new List<Filter>
            {
                new RegionFilter
                {
                    Value = "North America"
                }
            };

            ConverterInfo.BuildConverterInfo(Assembly.GetExecutingAssembly());

            var jsonFilters = FilterSerializer.Serialize(filters);
            filters = FilterSerializer.Deserialize<List<Filter>>(jsonFilters);

            var regionFilter = new RegionFilter((Filter<string>)filters.First(f => f.Name == "Region"));

            regionFilter.Should().NotBeNull();
            regionFilter.FilterType.Should().Be(FilterType.String);
            regionFilter.InputType.Should().Be(InputType.Select);
            regionFilter.Value.Should().Be("North America");

            var plantInfos = FakePlantInfos.Get().AsQueryable();
            var filteredPlantInfos = plantInfos.Where(regionFilter.Filter).ToList();

            filteredPlantInfos.Count.Should().Be(1);
        }
    }
}
