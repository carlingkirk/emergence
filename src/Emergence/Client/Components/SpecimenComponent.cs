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
        [Parameter]
        public Specimen SpecimenParam { get; set; }
        public Specimen Specimen { get; set; }
        public Origin SelectedOrigin { get; set; }
        public Lifeform SelectedLifeform { get; set; }
        public string OriginSearch { get; set; }
        public IEnumerable<SpecimenStage> SpecimenStages => Enum.GetValues(typeof(SpecimenStage)).Cast<SpecimenStage>();
        public IEnumerable<ItemType> ItemTypes => Enum.GetValues(typeof(ItemType)).Cast<ItemType>();
        public IEnumerable<Status> Statuses => Enum.GetValues(typeof(Status)).Cast<Status>();

        protected override async Task OnInitializedAsync()
        {
            if (Id > 0)
            {
                Specimen = await Client.GetFromJsonAsync<Specimen>($"/api/specimen/{Id}");
            }
            else if (SpecimenParam != null)
            {
                Specimen = SpecimenParam;
                SelectedLifeform = Specimen.Lifeform;
                if (SpecimenParam.Lifeform != null)
                {
                    Specimen.InventoryItem.Name = SpecimenParam.Lifeform.ScientificName;
                }
            }
            else if (Specimen == null)
            {
                Specimen = new Specimen
                {
                    Lifeform = new Lifeform(),
                    PlantInfo = new PlantInfo(),
                    InventoryItem = new InventoryItem()
                };
            }
        }

        protected async Task SaveSpecimen()
        {
            if (Specimen.SpecimenId == 0)
            {
                Specimen.DateCreated = DateTime.UtcNow;
            }
            Specimen.DateModified = DateTime.UtcNow;

            if (SelectedOrigin != null)
            {
                Specimen.InventoryItem.Origin = SelectedOrigin;
            }

            var result = await Client.PutAsJsonAsync("/api/specimen", Specimen);
            if (result.IsSuccessStatusCode)
            {
                SpecimenParam = Specimen = await result.Content.ReadFromJsonAsync<Specimen>();
            }
            BlazoredModal.Close(ModalResult.Ok(SpecimenParam));
        }

        protected async Task<IEnumerable<Origin>> FindOrigins(string searchText)
        {
            var origins = (await Client.GetFromJsonAsync<IEnumerable<Origin>>($"/api/origin/find?search={searchText}&skip=0&take=10")).ToList();

            origins.Add(new Origin { Name = searchText });
            return origins;
        }

        protected async Task<IEnumerable<Lifeform>> FindLifeforms(string searchText)
        {
            var lifeforms = (await Client.GetFromJsonAsync<IEnumerable<Lifeform>>($"/api/lifeform/find?search={searchText}&skip=0&take=10")).ToList();

            return lifeforms;
        }

        protected void SaveSelectedOrigin()
        {
            if (SelectedOrigin == null)
            {
                SelectedOrigin = new Origin
                {
                    Name = OriginSearch
                };
            }
            else if (SelectedOrigin.Name != OriginSearch)
            {
                SelectedOrigin.Name = OriginSearch;
            }
        }

        protected void PopulateInventoryItemName()
        {
            if (SelectedLifeform != null && string.IsNullOrEmpty(Specimen.InventoryItem.Name))
            {
                Specimen.InventoryItem.Name = SelectedLifeform.ScientificName;
            }
        }
    }
}
