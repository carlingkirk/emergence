using System.Linq;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Client.Components
{
    public class ListPlantInfosComponent : ListComponent<PlantInfo>
    {
        public override async Task<FindResult<PlantInfo>> GetListAsync(string searchText, int? skip = 0, int? take = 10, string sortBy = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            var result = await ApiClient.FindPlantInfosAsync(SearchText, skip, Take, SortBy, SortDirection);
            return new FindResult<PlantInfo>
            {
                Results = result.Results,
                Count = result.Count
            };
        }

        protected async Task UpdatePlantInfoAsync(PlantInfo plantInfo)
        {
            var result = await ModalServiceClient.ShowPlantInfoModal(plantInfo);
            plantInfo = List.Where(p => p.PlantInfoId == plantInfo.PlantInfoId).First();
            plantInfo = result.Data as PlantInfo;
        }
    }
}
