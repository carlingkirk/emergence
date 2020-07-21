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
    public class EditOriginComponent : ComponentBase
    {
        [Inject]
        protected HttpClient Client { get; set; }
        [CascadingParameter]
        protected BlazoredModalInstance BlazoredModal { get; set; }
        [Parameter]
        public int Id { get; set; }
        public Origin Origin { get; set; }
        public string OriginUri { get; set; }
        public IEnumerable<OriginType> OriginTypes => Enum.GetValues(typeof(OriginType)).Cast<OriginType>();

        protected override async Task OnInitializedAsync()
        {
            if (Id > 0)
            {
                var result = await Client.GetAsync($"/api/origin/{Id}");

                if (result.IsSuccessStatusCode)
                {
                    Origin = await result.Content.ReadFromJsonAsync<Origin>();
                }
                else
                {
                    var message = result.Content.ReadAsStringAsync();
                    throw new Exception(result.StatusCode + ": " + message);
                }
            }
            else
            {
                Origin = new Origin
                {
                    ParentOrigin = null,
                    Location = new Location()
                };
            }
        }

        protected async Task SaveOrigin()
        {
            if (Origin.OriginId == 0)
            {
                Origin.DateCreated = DateTime.UtcNow;
            }
            Origin.DateModified = DateTime.UtcNow;

            var result = await Client.PutAsJsonAsync("/api/origin", Origin);
            if (result.IsSuccessStatusCode)
            {
                Origin = await result.Content.ReadFromJsonAsync<Origin>();
            }

            if (BlazoredModal != null)
            {
                BlazoredModal.Close(ModalResult.Ok(Origin));
            }
        }
    }
}
