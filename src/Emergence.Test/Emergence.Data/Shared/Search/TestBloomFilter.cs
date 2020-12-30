using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Emergence.Data.Shared.Search;
using Emergence.Test.Data.Fakes.Stores;
using FluentAssertions;
using Xunit;

namespace Emergence.Test.Emergence.Data.Shared.Search
{
    public class TestBloomFilter
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
                new BloomFilter
                {
                    MinimumValue = 5,
                    MaximumValue = 7
                }
            };
            ConverterInfo.BuildConverterInfo(Assembly.GetExecutingAssembly());
            var jsonFilters = FilterSerializer.Serialize(filters);

            filters = FilterSerializer.Deserialize<List<Filter>>(jsonFilters);

            var bloomFilter = new BloomFilter((RangeFilter<int>)filters.First(f => f.Name == "Bloom"));

            bloomFilter.Should().NotBeNull();
            bloomFilter.FilterType.Should().Be(FilterType.Integer);
            bloomFilter.InputType.Should().Be(InputType.SelectRange);
            bloomFilter.MinimumValue.Should().Be(5);
            bloomFilter.MaximumValue.Should().Be(7);
        }

        [Theory]
        [InlineData(7, 8, 2)]
        [InlineData(4, 5, 1)]
        [InlineData(0, 5, 1)]
        [InlineData(8, 12, 2)]
        public void TestFilter(short minValue, short maxValue, int count)
        {
            var filter = new RangeFilter<int>
            {
                MinimumValue = minValue,
                MaximumValue = maxValue
            };

            var bloomFilter = new BloomFilter(filter);
            var plantInfos = FakePlantInfos.Get().AsQueryable();
            var filteredPlantInfos = plantInfos.Where(bloomFilter.Filter).ToList();

            filteredPlantInfos.Count.Should().Be(count);
        }
    }
}
