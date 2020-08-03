using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Client.Common;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class ListPlantInfosComponent : ListComponent<PlantInfo>
    {
        [Inject]
        protected IApiClient ApiClient { get; set; }
        [Inject]
        protected IModalServiceClient ModalServiceClient { get; set; }
        public IEnumerable<PlantInfo> PlantInfos { get; set; }

        public override async Task<FindResult<PlantInfo>> GetList(string searchText, int? skip = 0, int? take = 10, string sortBy = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            var result = await ApiClient.FindPlantInfosAsync(SearchText, skip, Take, SortBy, SortDirection);
            return new FindResult<PlantInfo>
            {
                Results = result.Results,
                Count = result.Count
            };
        }

        protected async Task UpdatePlantInfo(PlantInfo plantInfo)
        {
            var result = await ModalServiceClient.ShowPlantInfoModal(plantInfo);
            plantInfo = List.Where(p => p.PlantInfoId == plantInfo.PlantInfoId).First();
            plantInfo = result.Data as PlantInfo;
        }
    }
}
