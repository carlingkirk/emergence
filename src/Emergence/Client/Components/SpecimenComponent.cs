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
    public class SpecimenComponent : ComponentBase
    {
        [Inject]
        protected HttpClient Client { get; set; }
        [CascadingParameter]
        protected BlazoredModalInstance BlazoredModal { get; set; }
        [Parameter]
        public int Id { get; set; }
        public Specimen Specimen { get; set; }
        public IEnumerable<SpecimenStage> SpecimenStages => Enum.GetValues(typeof(SpecimenStage)).Cast<SpecimenStage>();
        public IEnumerable<ItemType> ItemTypes => Enum.GetValues(typeof(ItemType)).Cast<ItemType>();
        public IEnumerable<Status> Statuses => Enum.GetValues(typeof(Status)).Cast<Status>();

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
                    PlantInfo = new PlantInfo(),
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

        public async Task ShowModal(IModalService modalService, int id)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Id", id);
            var specimenModal = modalService.Show<Pages.EditSpecimen>("setgh", modalParams);
            var result = await specimenModal.Result;

            if (result.Cancelled)
            {
                Console.WriteLine("Modal was cancelled");
            }
            else
            {
                Console.WriteLine("Modal was closed");
            }
        }
    }
}
