using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Emergence.Data.Shared.Search;
using Emergence.Test.Data.Fakes.Stores;
using FluentAssertions;
using Xunit;

namespace Emergence.Test.Emergence.Data.Shared.Search
{
    public class TestSpreadFilter
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
                new RegionFilter
                {
                    Value = "North America"
                }
            };
            ConverterInfo.BuildConverterInfo(Assembly.GetExecutingAssembly());
            var jsonFilters = FilterSerializer.Serialize(filters);

            filters = FilterSerializer.Deserialize<List<Filter>>(jsonFilters);

            var spreadFilter = new SpreadFilter((RangeFilter<double?>)filters.First(f => f.Name == "Spread"));

            spreadFilter.Should().NotBeNull();
            spreadFilter.FilterType.Should().Be(FilterType.Double);
            spreadFilter.InputType.Should().Be(InputType.SelectRange);
            spreadFilter.MinimumValue.Should().Be(0.5);
            spreadFilter.MaximumValue.Should().Be(2);

            var plantInfos = FakePlantInfos.Get().AsQueryable();
            var filteredPlantInfos = plantInfos.Where(spreadFilter.Filter).ToList();

            filteredPlantInfos.Count.Should().Be(2);
        }
    }
}
