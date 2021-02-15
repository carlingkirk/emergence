using System;
using System.Linq;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Extensions;
using Emergence.Test.Data.Fakes.Stores;
using FluentAssertions;
using Xunit;

namespace Emergence.Test.Emergence.Data.Shared.Extensions
{
    public class TestPlantInfoExtensions
    {
        [Fact]
        public void TestAsModel()
        {
            var plantInfos = FakePlantInfos.Get();
            var plantInfoModels = plantInfos.Select(p => p.AsModel());

            var firstPlantInfo = plantInfoModels.First();
            firstPlantInfo.CommonName.Should().Be("Dense Blazing Star");
            firstPlantInfo.ScientificName.Should().Be("Liatris spicata");
            firstPlantInfo.BloomTime.MinimumBloomTime.Should().Be(Month.Jul);
            firstPlantInfo.BloomTime.MaximumBloomTime.Should().Be(Month.Aug);
            firstPlantInfo.Height.MinimumHeight.Should().Be(1);
            firstPlantInfo.Height.MaximumHeight.Should().Be(2);
            firstPlantInfo.Height.Unit.Should().Be(DistanceUnit.Feet);
            firstPlantInfo.Spread.MinimumSpread.Should().Be(0.75);
            firstPlantInfo.Spread.MaximumSpread.Should().Be(1.5);
            firstPlantInfo.Spread.Unit.Should().Be(DistanceUnit.Feet);
            firstPlantInfo.Requirements.WaterRequirements.MinimumWater.Should().Be(WaterType.Medium);
            firstPlantInfo.Requirements.WaterRequirements.MaximumWater.Should().Be(WaterType.Medium);
            firstPlantInfo.Requirements.LightRequirements.MinimumLight.Should().Be(LightType.FullShade);
            firstPlantInfo.Requirements.LightRequirements.MaximumLight.Should().Be(LightType.FullSun);
            firstPlantInfo.Requirements.ZoneRequirements.MinimumZone.Id.Should().Be(7);
            firstPlantInfo.Requirements.ZoneRequirements.MaximumZone.Id.Should().Be(22);
            firstPlantInfo.Requirements.StratificationStages.Should().HaveCount(1);
            firstPlantInfo.Requirements.StratificationStages.Count(s => s.Step == 1).Should().Be(1);
            firstPlantInfo.Requirements.StratificationStages.Count(s => s.DayLength == 30).Should().Be(1);
            firstPlantInfo.Requirements.StratificationStages.Count(s => s.StratificationType == StratificationType.ColdMoist).Should().Be(1);
            firstPlantInfo.WildlifeEffects.Should().HaveCount(1);
            firstPlantInfo.WildlifeEffects.Count(we => we.Wildlife == Wildlife.Bees).Should().Be(1);
            firstPlantInfo.WildlifeEffects.Count(we => we.Effect == Effect.Food).Should().Be(1);
            firstPlantInfo.Notes.Contains("feather").Should().BeTrue();
            firstPlantInfo.Visibility.Should().Be(Visibility.Contacts);
            firstPlantInfo.CreatedBy.Should().Be(Helpers.UserId);
            firstPlantInfo.ModifiedBy.Should().Be(Helpers.UserId);
            firstPlantInfo.DateCreated.Should().NotBeNull();
            firstPlantInfo.DateModified.Should().NotBeNull();
            firstPlantInfo.User.DisplayName.Should().Be("Belvedere");
            firstPlantInfo.Locations.Should().HaveCount(1);
            firstPlantInfo.Lifeform.ScientificName.Should().Be("Liatris spicata");
        }

        [Fact]
        public void TestAsSearchModel()
        {
            var plantInfos = FakePlantInfos.Get();
            var synonyms = FakeSynonyms.Get();
            var plantInfoModels = plantInfos.Select(p => p.AsSearchModel(null, synonyms));

            var firstPlantInfo = plantInfoModels.First();
            firstPlantInfo.CommonName.Should().Be("Dense Blazing Star");
            firstPlantInfo.ScientificName.Should().Be("Liatris spicata");
            firstPlantInfo.MinimumBloomTime.Should().Be((short)Month.Jul);
            firstPlantInfo.MaximumBloomTime.Should().Be((short)Month.Aug);
            firstPlantInfo.MinimumHeight.Should().Be(1);
            firstPlantInfo.MaximumHeight.Should().Be(2);
            firstPlantInfo.HeightUnit.Should().Be(DistanceUnit.Feet);
            firstPlantInfo.MinimumSpread.Should().Be(0.75);
            firstPlantInfo.MaximumSpread.Should().Be(1.5);
            firstPlantInfo.SpreadUnit.Should().Be(DistanceUnit.Feet);
            firstPlantInfo.MinimumWater.Should().Be(WaterType.Medium);
            firstPlantInfo.MaximumWater.Should().Be(WaterType.Medium);
            firstPlantInfo.MinimumLight.Should().Be(LightType.FullShade);
            firstPlantInfo.MaximumLight.Should().Be(LightType.FullSun);
            firstPlantInfo.MinimumZone.Id.Should().Be(7);
            firstPlantInfo.MaximumZone.Id.Should().Be(22);
            firstPlantInfo.Zones.Any(z => z.Id == 10).Should().BeTrue();
            firstPlantInfo.StratificationStages.Should().HaveCount(1);
            firstPlantInfo.StratificationStages.Count(s => s.Step == 1).Should().Be(1);
            firstPlantInfo.StratificationStages.Count(s => s.DayLength == 30).Should().Be(1);
            firstPlantInfo.StratificationStages.Count(s => s.StratificationType == StratificationType.ColdMoist).Should().Be(1);
            firstPlantInfo.WildlifeEffects.Should().HaveCount(1);
            firstPlantInfo.WildlifeEffects.Count(we => we.Wildlife == Wildlife.Bees).Should().Be(1);
            firstPlantInfo.WildlifeEffects.Count(we => we.Effect == Effect.Food).Should().Be(1);
            firstPlantInfo.SoilTypes.Should().HaveCount(3);
            firstPlantInfo.SoilTypes.Count(s => s == SoilType.Loamy).Should().Be(1);
            firstPlantInfo.Notes.Contains("feather").Should().BeTrue();
            firstPlantInfo.Visibility.Should().Be(Visibility.Contacts);
            firstPlantInfo.CreatedBy.Should().Be(Helpers.UserId);
            firstPlantInfo.ModifiedBy.Should().Be(Helpers.UserId);
            firstPlantInfo.DateCreated.Should().BeAfter(new DateTime());
            firstPlantInfo.DateModified.Should().NotBeNull();
            firstPlantInfo.User.DisplayName.Should().Be("Belvedere");
            firstPlantInfo.PlantLocations.Should().HaveCount(1);
            firstPlantInfo.Lifeform.ScientificName.Should().Be("Liatris spicata");
            firstPlantInfo.Synonyms.Should().HaveCount(1);
        }
    }
}
