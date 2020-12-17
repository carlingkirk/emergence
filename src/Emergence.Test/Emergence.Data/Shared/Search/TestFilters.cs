using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Emergence.Data.Shared.Search;
using Emergence.Test.Data.Fakes.Stores;
using FluentAssertions;
using Xunit;

namespace Emergence.Test.Data.Shared.Search
{
    public class TestFilters
    {
        [Fact]
        public void TestRegionFilter()
        {
            var filters = new List<Filter>
            {
                new RegionFilter
                {
                    Value = "North America"
                }
            };

            var options = new JsonSerializerOptions();
            options.Converters.Add(new FilterTypeDiscriminator<string>());

            var jsonFilters = JsonSerializer.Serialize(filters, options: options);

            filters = JsonSerializer.Deserialize<List<Filter>>(jsonFilters, options: options);

            var regionFilter = new RegionFilter((Filter<string>)filters.First(f => f.Name == "Location"));

            regionFilter.Should().NotBeNull();
            regionFilter.FilterType.Should().Be(FilterType.String);
            regionFilter.InputType.Should().Be(InputType.Select);
            regionFilter.Value.Should().Be("North America");

            var plantInfos = FakePlantInfos.Get().AsQueryable();
            var filteredPlantInfos = plantInfos.Where(regionFilter.Filter).ToList();

            filteredPlantInfos.Count.Should().Be(1);
        }

        [Fact]
        public void TestStageFilter()
        {
            var filters = new List<Filter>
            {
                new StageFilter
                {
                    Value = "In Ground"
                }
            };

            var options = new JsonSerializerOptions();
            options.Converters.Add(new FilterTypeDiscriminator<string>());

            var jsonFilters = JsonSerializer.Serialize(filters, options: options);

            filters = JsonSerializer.Deserialize<List<Filter>>(jsonFilters, options: options);

            var stageFilter = new StageFilter((Filter<string>)filters.First(f => f.Name == "Stage"));

            stageFilter.Should().NotBeNull();
            stageFilter.FilterType.Should().Be(FilterType.String);
            stageFilter.InputType.Should().Be(InputType.Select);
            stageFilter.Value.Should().Be("Seed");

            var specimens = FakeSpecimens.Get().AsQueryable();
            var filteredPlantInfos = specimens.Where(stageFilter.Filter).ToList();

            filteredPlantInfos.Count.Should().Be(2);
        }
    }
}
