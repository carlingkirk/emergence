using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class ActivityComponent : ViewerComponent<Activity>
    {
        [Parameter]
        public Activity Activity { get; set; }
        [Parameter]
        public Specimen SelectedSpecimen { get; set; }
        public IList<Photo> UploadedPhotos { get; set; }
        public IEnumerable<ActivityType> ActivityTypes => Enum.GetValues(typeof(ActivityType)).Cast<ActivityType>();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (Id > 0 || Activity != null)
            {
                Activity ??= await ApiClient.GetActivityAsync(Id);

                if (Activity.Photos != null && Activity.Photos.Any())
                {
                    UploadedPhotos = Activity.Photos.ToList();
                }
                else
                {
                    UploadedPhotos = new List<Photo>();
                }
                SelectedSpecimen = Activity.Specimen ?? null;
            }
            else
            {
                IsEditing = true;
                Activity = new Activity
                {
                    CreatedBy = UserId,
                    AssignedTo = UserId,
                    DateCreated = DateTime.Now,
                    Specimen = SelectedSpecimen
                };
                UploadedPhotos = new List<Photo>();
            }

            if (!string.IsNullOrEmpty(UserId) && Activity.CreatedBy == UserId)
            {
                IsEditable = true;
            }
        }

        protected async Task RemoveActivity()
        {
            var result = await ApiClient.RemoveActivityAsync(Activity);
            if (result)
            {
                Activity = null;

                await UnloadItem();
            }
        }
    }
}
