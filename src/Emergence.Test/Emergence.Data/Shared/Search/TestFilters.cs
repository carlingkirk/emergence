using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            ConverterInfo.BuildConverterInfo(Assembly.GetExecutingAssembly());

            var jsonFilters = FilterSerializer.Serialize(filters);
            filters = FilterSerializer.Deserialize<List<Filter>>(jsonFilters);

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
                    Value = "Seed"
                }
            };

            ConverterInfo.BuildConverterInfo(Assembly.GetExecutingAssembly());

            var jsonFilters = FilterSerializer.Serialize(filters);
            filters = FilterSerializer.Deserialize<List<Filter>>(jsonFilters);

            var stageFilter = new StageFilter((Filter<string>)filters.First(f => f.Name == "Stage"));

            stageFilter.Should().NotBeNull();
            stageFilter.FilterType.Should().Be(FilterType.String);
            stageFilter.InputType.Should().Be(InputType.Select);
            stageFilter.Value.Should().Be("Seed");

            var specimens = FakeSpecimens.Get().AsQueryable();
            var filteredPlantInfos = specimens.Where(stageFilter.Filter).ToList();

            filteredPlantInfos.Count.Should().Be(2);
        }

        [Fact]
        public void TestHeightFilter()
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

            var heightFilter = new HeightFilter((RangeFilter<double>)filters.First(f => f.Name == "Height"));

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
