using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class ListActivitiesComponent : ListComponent<Activity>
    {
        [Parameter]
        public Specimen Specimen { get; set; }

        public override async Task<FindResult<Activity>> GetListAsync(FindParams findParams)
        {
            FindResult<Activity> result;
            if (Specimen != null)
            {
                result = await ApiClient.FindActivitiesAsync(Specimen, findParams);
            }
            else
            {
                result = await ApiClient.FindActivitiesAsync(findParams);
            }

            return result;
        }
    }
}
