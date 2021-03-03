using System.Linq;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;
using Emergence.Test.Data.Fakes.Stores;
using FluentAssertions;
using Xunit;
using SharedModels = Emergence.Data.Shared.Models;

namespace Emergence.Test.Data.Shared.Models
{
    public class TestPlantInfo
    {
        [Fact]
        public void TestSearchModel()
        {
            var plantInfos = FakePlantInfos.Get();

            var plantInfoSearch = plantInfos.Select(p => p.AsSearchModel(p.PlantLocations, null)).First();

            plantInfoSearch.BloomTimes.Any(b => b == Month.Jul).Should().BeTrue();
            plantInfoSearch.BloomTimes.Any(b => b == Month.Aug).Should().BeTrue();
            plantInfoSearch.Zones.Any(z => z.Id == 7).Should().BeTrue();
            plantInfoSearch.Zones.Any(z => z.Id == 12).Should().BeTrue();
            plantInfoSearch.Zones.Any(z => z.Id == 22).Should().BeTrue();
            plantInfoSearch.Zones.Should().HaveCount(16);
            plantInfoSearch.WaterTypes.Any(w => w == WaterType.Medium).Should().BeTrue();
            plantInfoSearch.LightTypes.Any(l => l == LightType.FullShade).Should().BeTrue();
            plantInfoSearch.LightTypes.Any(l => l == LightType.PartShade).Should().BeTrue();
            plantInfoSearch.LightTypes.Any(l => l == LightType.PartSun).Should().BeTrue();
            plantInfoSearch.LightTypes.Any(l => l == LightType.FullSun).Should().BeTrue();
            plantInfoSearch.SoilTypes.Should().HaveCount(3);
            plantInfoSearch.SoilTypes.Count(s => s == SoilType.Loamy).Should().Be(1);

            plantInfoSearch = plantInfos.Select(p => p.AsSearchModel(p.PlantLocations, null)).Skip(1).First();

            plantInfoSearch.BloomTimes.Any(b => b == Month.Apr).Should().BeTrue();
            plantInfoSearch.BloomTimes.Any(b => b == Month.May).Should().BeTrue();
            plantInfoSearch.Zones.Any(z => z.Id == 13).Should().BeTrue();
            plantInfoSearch.Zones.Any(z => z.Id == 17).Should().BeTrue();
            plantInfoSearch.Zones.Any(z => z.Id == 22).Should().BeTrue();
            plantInfoSearch.Zones.Should().HaveCount(10);
            plantInfoSearch.WaterTypes.Any(w => w == WaterType.Dry).Should().BeTrue();
            plantInfoSearch.WaterTypes.Any(w => w == WaterType.MediumDry).Should().BeTrue();
            plantInfoSearch.WaterTypes.Any(w => w == WaterType.Medium).Should().BeTrue();
            plantInfoSearch.LightTypes.Any(l => l == LightType.PartShade).Should().BeTrue();
            plantInfoSearch.LightTypes.Any(l => l == LightType.PartSun).Should().BeTrue();
            plantInfoSearch.LightTypes.Any(l => l == LightType.FullSun).Should().BeTrue();
            plantInfoSearch.WildlifeEffects.Count(we => we.Wildlife == Wildlife.Bees).Should().Be(1);
            plantInfoSearch.WildlifeEffects.Count(we => we.Effect == Effect.Food).Should().Be(1);
            plantInfoSearch.SoilTypes.Should().BeNull();
            plantInfoSearch.Notes.Contains("feather");
        }

        [Fact]
        public void TestSearchModel_MinimumInfo()
        {
            var plantInfo = new PlantInfo
            {
                Lifeform = new Lifeform
                {
                    ScientificName = "Pohlia longicolla"
                }
            };

            var plantInfoSearch = plantInfo.AsSearchModel(null, null);

            plantInfoSearch.Lifeform.ScientificName.Should().Be("Pohlia longicolla");
            plantInfoSearch.PlantLocations.Should().BeNull();
            plantInfoSearch.Zones.Should().BeNull();
            plantInfoSearch.BloomTimes.Should().BeNull();
            plantInfoSearch.WaterTypes.Should().BeNull();
            plantInfoSearch.LightTypes.Should().BeNull();
            plantInfoSearch.WildlifeEffects.Should().BeNull();
            plantInfoSearch.SoilTypes.Should().BeNull();
            plantInfoSearch.PlantLocations.Should().BeNull();
            plantInfoSearch.MinimumBloomTime.Should().BeNull();
            plantInfoSearch.MaximumBloomTime.Should().BeNull();
            plantInfoSearch.MinimumHeight.Should().BeNull();
            plantInfoSearch.MaximumHeight.Should().BeNull();
            plantInfoSearch.HeightUnit.Should().BeNull();
            plantInfoSearch.MinimumLight.Should().BeNull();
            plantInfoSearch.MaximumLight.Should().BeNull();
            plantInfoSearch.MinimumSpread.Should().BeNull();
            plantInfoSearch.MaximumSpread.Should().BeNull();
            plantInfoSearch.SpreadUnit.Should().BeNull();
            plantInfoSearch.MinimumWater.Should().BeNull();
            plantInfoSearch.MaximumWater.Should().BeNull();
            plantInfoSearch.MinimumZone.Should().BeNull();
            plantInfoSearch.MaximumZone.Should().BeNull();
            plantInfoSearch.StratificationStages.Should().BeNull();
            plantInfoSearch.CommonName.Should().BeNull();
            plantInfoSearch.ScientificName.Should().BeNull();
        }

        [Fact]
        public void TestSearchModelFromModel()
        {
            var plantInfos = FakePlantInfos.Get().Select(p => p.AsModel());

            var plantInfoSearch = plantInfos.Select(p => p.AsSearchModel(null, null)).First();

            plantInfoSearch.BloomTimes.Any(b => b == Month.Jul).Should().BeTrue();
            plantInfoSearch.BloomTimes.Any(b => b == Month.Aug).Should().BeTrue();
            plantInfoSearch.Zones.Any(z => z.Id == 7).Should().BeTrue();
            plantInfoSearch.Zones.Any(z => z.Id == 12).Should().BeTrue();
            plantInfoSearch.Zones.Any(z => z.Id == 22).Should().BeTrue();
            plantInfoSearch.Zones.Should().HaveCount(16);
            plantInfoSearch.WaterTypes.Any(w => w == WaterType.Medium).Should().BeTrue();
            plantInfoSearch.LightTypes.Any(l => l == LightType.FullShade).Should().BeTrue();
            plantInfoSearch.LightTypes.Any(l => l == LightType.PartShade).Should().BeTrue();
            plantInfoSearch.LightTypes.Any(l => l == LightType.PartSun).Should().BeTrue();
            plantInfoSearch.LightTypes.Any(l => l == LightType.FullSun).Should().BeTrue();
            plantInfoSearch.SoilTypes.Should().HaveCount(3);
            plantInfoSearch.SoilTypes.Count(s => s == SoilType.Loamy).Should().Be(1);

            plantInfoSearch = plantInfos.Select(p => p.AsSearchModel(p.Locations, null)).Skip(1).First();

            plantInfoSearch.BloomTimes.Any(b => b == Month.Apr).Should().BeTrue();
            plantInfoSearch.BloomTimes.Any(b => b == Month.May).Should().BeTrue();
            plantInfoSearch.Zones.Any(z => z.Id == 13).Should().BeTrue();
            plantInfoSearch.Zones.Any(z => z.Id == 17).Should().BeTrue();
            plantInfoSearch.Zones.Any(z => z.Id == 22).Should().BeTrue();
            plantInfoSearch.Zones.Should().HaveCount(10);
            plantInfoSearch.WaterTypes.Any(w => w == WaterType.Dry).Should().BeTrue();
            plantInfoSearch.WaterTypes.Any(w => w == WaterType.MediumDry).Should().BeTrue();
            plantInfoSearch.WaterTypes.Any(w => w == WaterType.Medium).Should().BeTrue();
            plantInfoSearch.LightTypes.Any(l => l == LightType.PartShade).Should().BeTrue();
            plantInfoSearch.LightTypes.Any(l => l == LightType.PartSun).Should().BeTrue();
            plantInfoSearch.LightTypes.Any(l => l == LightType.FullSun).Should().BeTrue();
            plantInfoSearch.WildlifeEffects.Count(we => we.Wildlife == Wildlife.Bees).Should().Be(1);
            plantInfoSearch.WildlifeEffects.Count(we => we.Effect == Effect.Food).Should().Be(1);
            plantInfoSearch.SoilTypes.Should().BeNull();
            plantInfoSearch.Notes.Contains("feather");
        }

        [Fact]
        public void TestSearchModelFromModel_MinimumInfo()
        {
            var plantInfo = new SharedModels.PlantInfo
            {
                Lifeform = new SharedModels.Lifeform
                {
                    ScientificName = "Pohlia longicolla"
                }
            };

            var plantInfoSearch = plantInfo.AsSearchModel(null, null);

            plantInfoSearch.Lifeform.ScientificName.Should().Be("Pohlia longicolla");
            plantInfoSearch.PlantLocations.Should().BeNull();
            plantInfoSearch.Zones.Should().BeNull();
            plantInfoSearch.BloomTimes.Should().BeNull();
            plantInfoSearch.WaterTypes.Should().BeNull();
            plantInfoSearch.LightTypes.Should().BeNull();
            plantInfoSearch.WildlifeEffects.Should().BeNull();
            plantInfoSearch.SoilTypes.Should().BeNull();
            plantInfoSearch.PlantLocations.Should().BeNull();
            plantInfoSearch.MinimumBloomTime.Should().BeNull();
            plantInfoSearch.MaximumBloomTime.Should().BeNull();
            plantInfoSearch.MinimumHeight.Should().BeNull();
            plantInfoSearch.MaximumHeight.Should().BeNull();
            plantInfoSearch.HeightUnit.Should().BeNull();
            plantInfoSearch.MinimumLight.Should().BeNull();
            plantInfoSearch.MaximumLight.Should().BeNull();
            plantInfoSearch.MinimumSpread.Should().BeNull();
            plantInfoSearch.MaximumSpread.Should().BeNull();
            plantInfoSearch.SpreadUnit.Should().BeNull();
            plantInfoSearch.MinimumWater.Should().BeNull();
            plantInfoSearch.MaximumWater.Should().BeNull();
            plantInfoSearch.MinimumZone.Should().BeNull();
            plantInfoSearch.MaximumZone.Should().BeNull();
            plantInfoSearch.StratificationStages.Should().BeNull();
            plantInfoSearch.CommonName.Should().BeNull();
            plantInfoSearch.ScientificName.Should().BeNull();
        }
    }
}
