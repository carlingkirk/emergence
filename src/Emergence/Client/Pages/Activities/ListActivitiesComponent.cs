using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Client.Common;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class ListActivitiesComponent : ListComponent<Activity>
    {
        [Parameter]
        public Specimen Specimen { get; set; }
        protected static Dictionary<string, string> Headers =>
            new Dictionary<string, string>
            {
                { "Name", "Name" },
                { "ScientificName", "Scientific Name" },
                { "ActivityType", "Activity Type" },
                { "DateOccured", "Date Occured" },
                { "DateScheduled", "Date Scheduled" }
            };

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
