using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.Modal;
using Blazored.Modal.Services;
using Emergence.Client.Common;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class EditActivityComponent : ComponentBase
    {
        [Inject]
        protected IApiClient ApiClient { get; set; }
        [CascadingParameter]
        protected BlazoredModalInstance BlazoredModal { get; set; }
        [Parameter]
        public int Id { get; set; }
        public Activity Activity { get; set; }
        public Specimen SelectedSpecimen { get; set; }
        public IEnumerable<ActivityType> ActivityTypes => Enum.GetValues(typeof(ActivityType)).Cast<ActivityType>();

        protected override async Task OnInitializedAsync()
        {
            if (Id > 0)
            {
                Activity = await ApiClient.GetActivityAsync(Id);
                SelectedSpecimen = Activity.Specimen ?? null;
            }
            else
            {
                Activity = new Activity();
            }
        }

        protected async Task SaveActivityAsync()
        {
            if (Activity.ActivityId == 0)
            {
                Activity.DateCreated = DateTime.UtcNow;
            }
            Activity.DateModified = DateTime.UtcNow;

            if (SelectedSpecimen != null)
            {
                Activity.Specimen = SelectedSpecimen;
            }

            Activity = await ApiClient.PutActivityAsync(Activity);
            Id = Activity.ActivityId;

            if (BlazoredModal != null)
            {
                BlazoredModal.Close(ModalResult.Ok(Activity));
            }
        }

        protected async Task<IEnumerable<Specimen>> FindSpecimensAsync(string searchText)
        {
            var specimens = (await ApiClient.FindSpecimensAsync(searchText)).ToList();
            var lifeforms = await ApiClient.FindLifeformsAsync(searchText, 0, 3);
            foreach (var lifeform in lifeforms)
            {
                specimens.Add(new Specimen { Lifeform = lifeform, InventoryItem = new InventoryItem() });
            }
            return specimens;
        }
    }
}