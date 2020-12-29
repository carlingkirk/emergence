using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Emergence.Data.Shared.Search;
using Emergence.Test.Data.Fakes.Stores;
using FluentAssertions;
using Xunit;

namespace Emergence.Test.Emergence.Data.Shared.Search
{
    public class TestLightFilter
    {
        [Fact]
        public void TestSerialization()
        {
            var filters = new List<Filter>
            {
                new SpreadFilter
                {
                    MinimumValue = 0.5,
                    MaximumValue = 2
                },
                new LightFilter
                {
                    MinimumValue = "FullShade",
                    MaximumValue = "PartShade"
                }
            };
            ConverterInfo.BuildConverterInfo(Assembly.GetExecutingAssembly());
            var jsonFilters = FilterSerializer.Serialize(filters);

            filters = FilterSerializer.Deserialize<List<Filter>>(jsonFilters);

            var lightFilter = new LightFilter((RangeFilter<string>)filters.First(f => f.Name == "Light"));

            lightFilter.Should().NotBeNull();
            lightFilter.FilterType.Should().Be(FilterType.String);
            lightFilter.InputType.Should().Be(InputType.SelectRange);
            lightFilter.MinimumValue.Should().Be("FullShade");
            lightFilter.MaximumValue.Should().Be("PartShade");
        }

        [Theory]
        [InlineData("PartShade", "FullSun", 3)]
        [InlineData("", "FullSun", 3)]
        [InlineData("FullShade", "", 3)]
        [InlineData("", "FullShade", 1)]
        [InlineData("FullShade", "PartSun", 3)]
        [InlineData("FullShade", "PartShade", 2)]
        public void TestFilter(string minValue, string maxValue, int count)
        {
            var filter = new RangeFilter<string>
            {
                MinimumValue = minValue,
                MaximumValue = maxValue
            };
            var lightFilter = new LightFilter(filter);
            var plantInfos = FakePlantInfos.Get().AsQueryable();
            var filteredPlantInfos = plantInfos.Where(lightFilter.Filter).ToList();
            filteredPlantInfos.Count.Should().Be(count);
        }
    }
}
