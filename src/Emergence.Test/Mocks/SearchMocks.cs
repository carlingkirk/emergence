using System.Collections.Generic;
using System.Linq;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Search.Models;
using Emergence.Service.Search;
using Moq;
using Models = Emergence.Data.Shared.Models;
using Stores = Emergence.Data.Shared.Stores;

namespace Emergence.Test.Mocks
{
    public static class SearchMocks
    {
        public static Mock<IIndex<PlantInfo, Models.PlantInfo>> GetStandardMockPlantInfoIndex(IEnumerable<Stores.PlantInfo> plantInfos = null)
        {
            if (plantInfos == null)
            {
                plantInfos = Data.Fakes.Stores.FakePlantInfos.Get();
            }
            var mockPlantInfoIndex = new Mock<IIndex<PlantInfo, Models.PlantInfo>>();
            mockPlantInfoIndex.Setup(pi => pi.SearchAsync(It.IsAny<FindParams<Models.PlantInfo>>(), It.IsAny<Models.User>()))
                .ReturnsAsync((PlantInfoFindParams findParams, Models.User user) => new SearchResponse<PlantInfo>
                {
                    Count = plantInfos.AsQueryable().CanViewContent(user).Count(),
                    Documents = plantInfos.AsQueryable().CanViewContent(user).Select(pi => pi.AsSearchModel(null, null))
                });

            return mockPlantInfoIndex;
        }

        public static Mock<IIndex<Lifeform, Models.Lifeform>> GetStandardMockLifeformIndex(IEnumerable<Stores.Lifeform> lifeforms = null)
        {
            if (lifeforms == null)
            {
                lifeforms = Data.Fakes.Stores.FakeLifeforms.Get();
            }
            var mockLifeformIndex = new Mock<IIndex<Lifeform, Models.Lifeform>>();
            mockLifeformIndex.Setup(pi => pi.SearchAsync(It.IsAny<FindParams<Models.Lifeform>>(), It.IsAny<Models.User>())).ReturnsAsync(new SearchResponse<Lifeform>
            {
                Count = lifeforms.Count(),
                Documents = lifeforms.Select(l => l.AsSearchModel())
            });

            return mockLifeformIndex;
        }
    }
}
