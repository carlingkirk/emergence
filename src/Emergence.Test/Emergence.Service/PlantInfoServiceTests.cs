using System.Linq;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;
using Emergence.Service;
using Emergence.Service.Search;
using Emergence.Test.Data.Fakes.Stores;
using Emergence.Test.Mocks;
using FluentAssertions;
using Moq;
using Xunit;
using SearchModels = Emergence.Data.Shared.Search.Models;

namespace Emergence.Test.API.Services
{
    public class PlantInfoServiceTests
    {
        private readonly Mock<IRepository<PlantInfo>> _mockPlantInfoRepository;
        private readonly Mock<IRepository<PlantLocation>> _mockPlantLocationRepository;
        private readonly Mock<IIndex<SearchModels.PlantInfo>> _mockPlantInfoIndex;

        public PlantInfoServiceTests()
        {
            _mockPlantInfoRepository = RepositoryMocks.GetStandardMockPlantInfoRepository();
            _mockPlantLocationRepository = RepositoryMocks.GetStandardMockPlantLocationRepository();
            _mockPlantInfoIndex = SearchMocks.GetStandardMockPlantInfoIndex();
        }

        [Fact]
        public async Task TestGetPlantInfoAsync()
        {
            var plantInfoService = new PlantInfoService(_mockPlantInfoRepository.Object, _mockPlantLocationRepository.Object, _mockPlantInfoIndex.Object);
            var plantInfo = await plantInfoService.GetPlantInfoAsync(1, FakeUsers.GetPublic().AsModel());

            plantInfo.Should().NotBeNull("it exists");
        }

        [Fact]
        public async Task TestAddOrUpdatePlantInfoAsync()
        {
            var plantInfoService = new PlantInfoService(_mockPlantInfoRepository.Object, _mockPlantLocationRepository.Object, _mockPlantInfoIndex.Object);
            var plantInfo = FakePlantInfos.Get().First().AsModel();

            var plantInfoResult = await plantInfoService.AddOrUpdatePlantInfoAsync(plantInfo);

            plantInfoResult.Should().NotBeNull("it exists");
        }

        [Fact]
        public async Task TestFindPlantInfosAsync()
        {
            var plantInfoService = new PlantInfoService(_mockPlantInfoRepository.Object, _mockPlantLocationRepository.Object, _mockPlantInfoIndex.Object);

            var plantInfoResult = await plantInfoService.FindPlantInfos(new FindParams
            {
                SearchText = "Liatris spicata",
                Skip = 0,
                Take = 10,
                SortBy = "",
                SortDirection = SortDirection.None
            },
            FakeUsers.GetPublic().AsModel());

            plantInfoResult.Results.Should().NotBeNull("it exists");
            plantInfoResult.Count.Should().Be(3);
            plantInfoResult.Results.Should().HaveCount(3);
        }
    }
}
