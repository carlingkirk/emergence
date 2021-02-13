using System.Linq;
using Emergence.Data.Shared.Extensions;
using Emergence.Test.Data.Fakes.Stores;
using FluentAssertions;
using Xunit;
using SharedModels = Emergence.Data.Shared;

namespace Emergence.Test.Data.Shared.Models
{
    public class TestPlantInfo
    {
        [Fact]
        public void TestSearchModel()
        {
            var plantInfos = FakePlantInfos.Get();

            var plantInfoSearch = plantInfos.Select(p => p.AsSearchModel(p.PlantLocations, null)).First();

            plantInfoSearch.BloomTimes.Any(b => b == SharedModels.Month.Jul).Should().BeTrue();
            plantInfoSearch.BloomTimes.Any(b => b == SharedModels.Month.Aug).Should().BeTrue();
            plantInfoSearch.Zones.Any(z => z.Id == 7).Should().BeTrue();
            plantInfoSearch.Zones.Any(z => z.Id == 12).Should().BeTrue();
            plantInfoSearch.Zones.Any(z => z.Id == 22).Should().BeTrue();
            plantInfoSearch.Zones.Should().HaveCount(16);
            plantInfoSearch.WaterTypes.Any(w => w == SharedModels.WaterType.Medium).Should().BeTrue();
            plantInfoSearch.LightTypes.Any(l => l == SharedModels.LightType.FullShade).Should().BeTrue();
            plantInfoSearch.LightTypes.Any(l => l == SharedModels.LightType.PartShade).Should().BeTrue();
            plantInfoSearch.LightTypes.Any(l => l == SharedModels.LightType.PartSun).Should().BeTrue();
            plantInfoSearch.LightTypes.Any(l => l == SharedModels.LightType.FullSun).Should().BeTrue();

            plantInfoSearch = plantInfos.Select(p => p.AsSearchModel(p.PlantLocations, null)).Skip(1).First();

            plantInfoSearch.BloomTimes.Any(b => b == SharedModels.Month.Apr).Should().BeTrue();
            plantInfoSearch.BloomTimes.Any(b => b == SharedModels.Month.May).Should().BeTrue();
            plantInfoSearch.Zones.Any(z => z.Id == 13).Should().BeTrue();
            plantInfoSearch.Zones.Any(z => z.Id == 17).Should().BeTrue();
            plantInfoSearch.Zones.Any(z => z.Id == 22).Should().BeTrue();
            plantInfoSearch.Zones.Should().HaveCount(10);
            plantInfoSearch.WaterTypes.Any(w => w == SharedModels.WaterType.Dry).Should().BeTrue();
            plantInfoSearch.WaterTypes.Any(w => w == SharedModels.WaterType.MediumDry).Should().BeTrue();
            plantInfoSearch.WaterTypes.Any(w => w == SharedModels.WaterType.Medium).Should().BeTrue();
            plantInfoSearch.LightTypes.Any(l => l == SharedModels.LightType.PartShade).Should().BeTrue();
            plantInfoSearch.LightTypes.Any(l => l == SharedModels.LightType.PartSun).Should().BeTrue();
            plantInfoSearch.LightTypes.Any(l => l == SharedModels.LightType.FullSun).Should().BeTrue();
            plantInfoSearch.WildlifeEffects.Count(we => we.Wildlife == SharedModels.Wildlife.Bees).Should().Be(1);
            plantInfoSearch.WildlifeEffects.Count(we => we.Effect == SharedModels.Effect.Food).Should().Be(1);
            plantInfoSearch.Notes.Contains("feather");
        }
    }
}
