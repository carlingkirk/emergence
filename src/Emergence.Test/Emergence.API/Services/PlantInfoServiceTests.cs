using System.Linq;
using System.Threading.Tasks;
using Emergence.API.Services;
using Emergence.Data;
using Emergence.Data.Shared.Stores;
using Emergence.Test.Mocks;
using FluentAssertions;
using Moq;
using Xunit;
using Models = Emergence.Data.Shared.Models;

namespace Emergence.Test.API.Services
{
    public class PlantInfoServiceTests
    {
        private readonly Mock<IRepository<PlantInfo>> _mockPlantInfoRepository;

        public PlantInfoServiceTests()
        {
            _mockPlantInfoRepository = RepositoryMocks.GetStandardMockPlantInfoRepository();
        }

        [Fact]
        public async Task TestGetPlantInfoAsync()
        {
            var plantInfoService = new PlantInfoService(_mockPlantInfoRepository.Object);
            var plantInfo = await plantInfoService.GetPlantInfoAsync(1);

            plantInfo.Should().NotBeNull("it exists");
        }

        [Fact]
        public async Task TestGetPlantInfosAsync()
        {
            var plantInfoService = new PlantInfoService(_mockPlantInfoRepository.Object);
            var plantInfos = await plantInfoService.GetPlantInfosAsync();

            plantInfos.Should().NotBeNull("it exists");
            plantInfos.Should().HaveCount(2);
            plantInfos.Where(p => p.BloomTime.MinimumBloomTime == Models.Month.Jul).Should().HaveCount(1);
            plantInfos.Where(p => p.Requirements.StratificationStages.Any(s => s.StratificationType == Models.StratificationType.AbrasionScarify)).Should().HaveCount(1);
        }

        [Fact]
        public async Task TestAddOrUpdatePlantInfoAsync()
        {
            var plantInfoService = new PlantInfoService(_mockPlantInfoRepository.Object);
            var plantInfo = Data.Fakes.Models.FakePlantInfos.Get().First();

            var plantInfoResult = await plantInfoService.AddOrUpdatePlantInfoAsync(plantInfo);

            plantInfoResult.Should().NotBeNull("it exists");
        }
    }
}
