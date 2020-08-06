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
            if (!result.Cancelled)
            {
                plantInfo = List.Where(p => p.PlantInfoId == plantInfo.PlantInfoId).First();
                plantInfo = result.Data as PlantInfo;
            }
        }

        protected async Task UpdateOriginAsync(Origin origin)
        {
            var result = await ModalServiceClient.ShowOriginModal(origin);
            if (!result.Cancelled)
            {
                List.Where(p => p.Origin.OriginId == origin.OriginId).ToList().ForEach(p =>
                {
                    p.Origin = result.Data as Origin;
                });
            }
        }
    }
}
