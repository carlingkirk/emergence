using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Emergence.Data.Shared.Search;
using Emergence.Test.Data.Fakes.Stores;
using FluentAssertions;
using Xunit;

namespace Emergence.Test.Emergence.Data.Shared.Search
{
    public class TestStageFilter
    {
        [Fact]
        public void TestSerialization()
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
    }
}
