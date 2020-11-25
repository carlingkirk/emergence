using System;
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
        [Parameter]
        public bool ContactsOnly { get; set; }
        [Parameter]
        public bool Upcoming { get; set; }
        [Parameter]
        public bool Single { get; set; }
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
            findParams.ContactsOnly = ContactsOnly;
            if (Specimen != null)
            {
                result = await ApiClient.FindActivitiesAsync(findParams, Specimen);
            }
            else if (Upcoming)
            {
                result = await ApiClient.FindScheduledActivitiesAsync(findParams, DateTime.UtcNow);
            }
            else
            {
                result = await ApiClient.FindActivitiesAsync(findParams);
            }

            return result;
        }
    }
}
