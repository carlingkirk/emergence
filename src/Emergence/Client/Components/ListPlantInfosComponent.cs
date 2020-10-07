using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Client.Components
{
    public class ListPlantInfosComponent : ListComponent<PlantInfo>
    {
        public override async Task<FindResult<PlantInfo>> GetListAsync(FindParams findParams)
        {
            var result = await ApiClient.FindPlantInfosAsync(findParams);

            return new FindResult<PlantInfo>
            {
                Results = result.Results,
                Count = result.Count
            };
        }
    }
}
