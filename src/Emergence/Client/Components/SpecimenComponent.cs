using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.Modal;
using Blazored.Modal.Services;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class SpecimenComponent : ComponentBase
    {
        [Inject]
        protected HttpClient Client { get; set; }
        [CascadingParameter]
        protected BlazoredModalInstance BlazoredModal { get; set; }
        [Parameter]
        public int Id { get; set; }
        public Specimen Specimen { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (Id > 0)
            {
                Specimen = await Client.GetFromJsonAsync<Specimen>($"/api/specimen/{Id}");
            }
            else
            {
                Specimen = new Specimen
                {
                    Plant = new Plant(),
                    InventoryItem = new InventoryItem()
                };
            }
        }

        protected async Task SaveSpecimen()
        {
            var result = await Client.PutAsJsonAsync("/api/specimen", Specimen);
            if (result.IsSuccessStatusCode)
            {
                Specimen = await result.Content.ReadFromJsonAsync<Specimen>();
            }
            BlazoredModal.Close(ModalResult.Ok(Specimen));
        }
    }
}
