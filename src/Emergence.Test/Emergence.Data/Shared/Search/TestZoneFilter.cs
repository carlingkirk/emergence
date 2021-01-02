using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Emergence.Data.Shared.Search;
using Emergence.Test.Data.Fakes.Stores;
using FluentAssertions;
using Xunit;

namespace Emergence.Test.Emergence.Data.Shared.Search
{
    public class TestZoneFilter
    {
        [Fact]
        public void TestSerialization()
        {
            var filters = new List<Filter>
            {
                new ZoneFilter
                {
                    Value = 5
                }
            };

            ConverterInfo.BuildConverterInfo(Assembly.GetExecutingAssembly());

            var jsonFilters = FilterSerializer.Serialize(filters);
            filters = FilterSerializer.Deserialize<List<Filter>>(jsonFilters);

            var zoneFilter = new ZoneFilter((SelectFilter<int>)filters.First(f => f.Name == "Zone"));

            zoneFilter.Should().NotBeNull();
            zoneFilter.FilterType.Should().Be(FilterType.Integer);
            zoneFilter.InputType.Should().Be(InputType.Select);
            zoneFilter.Value.Should().Be(5);

            var plantInfos = FakePlantInfos.Get().AsQueryable();
            var filteredPlantInfos = plantInfos.Where(zoneFilter.Filter).ToList();

            filteredPlantInfos.Count.Should().Be(3);
        }

        [Theory]
        [InlineData(7, 3)]
        [InlineData(22, 3)]
        [InlineData(35, 0)]
        [InlineData(13, 3)]
        public void TestFilter(int value, int count)
        {
            var filter = new SelectFilter<int>
            {
                Value = value
            };

            var zoneFilter = new ZoneFilter(filter);
            var plantInfos = FakePlantInfos.Get().AsQueryable();
            var filteredPlantInfos = plantInfos.Where(zoneFilter.Filter).ToList();
            filteredPlantInfos.Count.Should().Be(count);
        }
    }
}
