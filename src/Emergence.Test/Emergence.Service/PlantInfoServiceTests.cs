using System.Linq;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Stores;
using Emergence.Service;
using Emergence.Test.Data.Fakes.Models;
using Emergence.Test.Mocks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Emergence.Test.API.Services
{
    public class PlantInfoServiceTests
    {
        private readonly Mock<IRepository<PlantInfo>> _mockPlantInfoRepository;
        private readonly Mock<IRepository<PlantLocation>> _mockPlantLocationRepository;

        public PlantInfoServiceTests()
        {
            _mockPlantInfoRepository = RepositoryMocks.GetStandardMockPlantInfoRepository();
            _mockPlantLocationRepository = RepositoryMocks.GetStandardMockPlantLocationRepository();
        }

        [Fact]
        public async Task TestGetPlantInfoAsync()
        {
            var plantInfoService = new PlantInfoService(_mockPlantInfoRepository.Object, _mockPlantLocationRepository.Object);
            var plantInfo = await plantInfoService.GetPlantInfoAsync(1, FakeUsers.Get().First());

            plantInfo.Should().NotBeNull("it exists");
        }

        [Fact]
        public async Task TestAddOrUpdatePlantInfoAsync()
        {
            var plantInfoService = new PlantInfoService(_mockPlantInfoRepository.Object, _mockPlantLocationRepository.Object);
            var plantInfo = FakePlantInfos.Get().First();

            var plantInfoResult = await plantInfoService.AddOrUpdatePlantInfoAsync(plantInfo);

            plantInfoResult.Should().NotBeNull("it exists");
        }

        [Fact]
        public async Task TestFindPlantInfosAsync()
        {
            var plantInfoService = new PlantInfoService(_mockPlantInfoRepository.Object, _mockPlantLocationRepository.Object);

            var plantInfoResult = await plantInfoService.FindPlantInfos(new FindParams
            {
                SearchText = "Liatris spicata",
                Skip = 0,
                Take = 10,
                SortBy = "",
                SortDirection = SortDirection.None,
            },
            FakeUsers.Get().First());

            plantInfoResult.Results.Should().NotBeNull("it exists");
            plantInfoResult.Count.Should().Be(2);
            plantInfoResult.Results.Should().HaveCount(2);
        }
    }
}
