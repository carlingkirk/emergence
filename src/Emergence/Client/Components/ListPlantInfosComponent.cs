using System.Linq;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Client.Components
{
    public class ListPlantInfosComponent : ListComponent<PlantInfo>
    {
        public override async Task<FindResult<PlantInfo>> GetListAsync(FindParams findParams)
        {
            var result = new FindResult<PlantInfo>();
            if (ExistingCascadedAuthenticationState != null)
            {
                var state = await ExistingCascadedAuthenticationState;
                if (state.User.Identity.IsAuthenticated)
                {
                    result = await ApiClient.FindPlantInfosAsync(findParams);
                }
                else
                {
                    result = await ApiClient.FindPublicPlantInfosAsync(findParams);
                }
            }

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
            var result = await ModalServiceClient.ShowOriginModal(origin.OriginId);
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
