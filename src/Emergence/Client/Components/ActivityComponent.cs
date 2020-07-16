using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.Modal;
using Blazored.Modal.Services;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class ActivityComponent : ComponentBase
    {
        [Inject]
        protected HttpClient Client { get; set; }
        [CascadingParameter]
        protected BlazoredModalInstance BlazoredModal { get; set; }
        [Parameter]
        public int Id { get; set; }
        [Parameter]
        public IEnumerable<Activity> ActivitiesResult { get; set; }
        public Activity Activity { get; set; }
        public Specimen SelectedSpecimen { get; set; }
        public IEnumerable<ActivityType> ActivityTypes => Enum.GetValues(typeof(ActivityType)).Cast<ActivityType>();

        protected override async Task OnInitializedAsync()
        {
            if (Id > 0)
            {
                Activity = await Client.GetFromJsonAsync<Activity>($"/api/activity/{Id}");
            }
            else
            {
                Activity = new Activity();
            }
        }

        protected async Task SaveActivity()
        {
            if (Activity.ActivityId == 0)
            {
                Activity.DateCreated = DateTime.UtcNow;
            }
            Activity.DateModified = DateTime.UtcNow;

            var result = await Client.PutAsJsonAsync("/api/activity", Activity);
            if (result.IsSuccessStatusCode)
            {
                Activity = await result.Content.ReadFromJsonAsync<Activity>();
            }
            BlazoredModal.Close(ModalResult.Ok(Activity));
        }

        protected async Task<IEnumerable<Specimen>> FindSpecimens(string searchText)
        {
            var response = (await Client.GetFromJsonAsync<IEnumerable<Specimen>>($"/api/specimen/find?search={searchText}&skip=0&take=10")).ToList();
            var lifeforms = await Client.GetFromJsonAsync<IEnumerable<Lifeform>>($"/api/lifeform/find?search={searchText}&skip=0&take=3");

            foreach (var lifeform in lifeforms)
            {
                response.Add(new Specimen { Lifeform = lifeform, InventoryItem = new InventoryItem() });
            }
            return response;
        }
    }
}
