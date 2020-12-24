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

        [Fact]
        public void TestSpreadFilter()
        {
            var filters = new List<Filter>
            {
                new SpreadFilter
                {
                    MinimumValue = 0.5,
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

            var spreadFilter = new SpreadFilter((RangeFilter<double>)filters.First(f => f.Name == "Spread"));

            spreadFilter.Should().NotBeNull();
            spreadFilter.FilterType.Should().Be(FilterType.Double);
            spreadFilter.InputType.Should().Be(InputType.SelectRange);
            spreadFilter.MinimumValue.Should().Be(0.5);
            spreadFilter.MaximumValue.Should().Be(2);

            var plantInfos = FakePlantInfos.Get().AsQueryable();
            var filteredPlantInfos = plantInfos.Where(spreadFilter.Filter).ToList();

            filteredPlantInfos.Count.Should().Be(2);
        }

        [Fact]
        public void TestLightFilter()
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

            var plantInfos = FakePlantInfos.Get().AsQueryable();
            var filteredPlantInfos = plantInfos.Where(lightFilter.Filter).ToList();

            filteredPlantInfos.Count.Should().Be(2);

            lightFilter = new LightFilter(new RangeFilter<string> { MinimumValue = "PartShade", MaximumValue = "FullSun" });
            filteredPlantInfos = plantInfos.Where(lightFilter.Filter).ToList();

            filteredPlantInfos.Count.Should().Be(3);

            lightFilter = new LightFilter(new RangeFilter<string> { MinimumValue = "", MaximumValue = "FullSun" });
            filteredPlantInfos = plantInfos.Where(lightFilter.Filter).ToList();

            filteredPlantInfos.Count.Should().Be(3);

            lightFilter = new LightFilter(new RangeFilter<string> { MinimumValue = "FullShade", MaximumValue = "" });
            filteredPlantInfos = plantInfos.Where(lightFilter.Filter).ToList();

            filteredPlantInfos.Count.Should().Be(3);

            lightFilter = new LightFilter(new RangeFilter<string> { MinimumValue = "", MaximumValue = "FullShade" });
            filteredPlantInfos = plantInfos.Where(lightFilter.Filter).ToList();

            filteredPlantInfos.Count.Should().Be(1);
        }
    }
}
