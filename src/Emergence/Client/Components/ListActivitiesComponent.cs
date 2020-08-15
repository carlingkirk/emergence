using System.Linq;
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

        public override async Task<FindResult<Activity>> GetListAsync(string searchText, int? skip = 0, int? take = 10, string sortBy = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            FindResult<Activity> result;

            if (Specimen != null)
            {
                result = await ApiClient.FindActivitiesAsync(Specimen, skip, Take, SortBy, SortDirection);
            }
            else
            {
                result = await ApiClient.FindActivitiesAsync(SearchText, skip, Take, SortBy, SortDirection);
            }

            return result;
        }

        protected async Task UpdateActivityAsync(Activity activity)
        {
            var result = await ModalServiceClient.ShowActivityModal(activity);

            if (!result.Cancelled)
            {
                activity = List.Where(a => a.ActivityId == activity.ActivityId).First();
                activity = result.Data as Activity;
            }
        }

        protected async Task UpdateSpecimenAsync(Specimen specimen)
        {
            var result = await ModalServiceClient.ShowSpecimenModal(specimen.SpecimenId);

            if (!result.Cancelled)
            {
                List.Where(a => a.Specimen.SpecimenId == specimen.SpecimenId).ToList().ForEach(a =>
                {
                    a.Specimen = result.Data as Specimen;
                });
            }
        }
    }
}
