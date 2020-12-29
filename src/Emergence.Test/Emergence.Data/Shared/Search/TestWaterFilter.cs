using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Emergence.Data.Shared.Search;
using Emergence.Test.Data.Fakes.Stores;
using FluentAssertions;
using Xunit;

namespace Emergence.Test.Emergence.Data.Shared.Search
{
    public class TestWaterFilter
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
                new WaterFilter
                {
                    MinimumValue = "Dry",
                    MaximumValue = "Wet"
                }
            };
            ConverterInfo.BuildConverterInfo(Assembly.GetExecutingAssembly());
            var jsonFilters = FilterSerializer.Serialize(filters);

            filters = FilterSerializer.Deserialize<List<Filter>>(jsonFilters);

            var lightFilter = new LightFilter((RangeFilter<string>)filters.First(f => f.Name == "Water"));

            lightFilter.Should().NotBeNull();
            lightFilter.FilterType.Should().Be(FilterType.String);
            lightFilter.InputType.Should().Be(InputType.SelectRange);
            lightFilter.MinimumValue.Should().Be("Dry");
            lightFilter.MaximumValue.Should().Be("Wet");
        }

        [Theory]
        [InlineData("Dry", "MediumDry", 1)]
        [InlineData("Dry", "Wet", 3)]
        [InlineData("Dry", "MediumWet", 3)]
        [InlineData("", "Medium", 3)]
        [InlineData("Medium", "", 3)]
        [InlineData("", "Dry", 1)]
        [InlineData("Dry", "", 3)]
        [InlineData("MediumWet", "Wet", 0)]
        public void TestFilter(string minValue, string maxValue, int count)
        {
            var filter = new RangeFilter<string>
            {
                MinimumValue = minValue,
                MaximumValue = maxValue
            };

            var waterFilter = new WaterFilter(filter);

            var plantInfos = FakePlantInfos.Get().AsQueryable();
            var filteredPlantInfos = plantInfos.Where(waterFilter.Filter).ToList();
            filteredPlantInfos.Count.Should().Be(count);
        }
    }
}
