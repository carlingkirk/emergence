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
    public class EditSpecimenComponent : ComponentBase
    {
        [Inject]
        protected IApiClient ApiClient { get; set; }
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
                Specimen = await ApiClient.GetSpecimenAsync(Id);
                SelectedOrigin = Specimen.InventoryItem.Origin ?? null;
                SelectedLifeform = Specimen.Lifeform;
            }
            else if (SpecimenParam != null)
            {
                Specimen = SpecimenParam;
                SelectedLifeform = Specimen.Lifeform;
                SelectedOrigin = Specimen.InventoryItem.Origin;
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

        protected async Task SaveSpecimenAsync()
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

            Specimen = await ApiClient.PutSpecimenAsync(Specimen);

            if (BlazoredModal != null)
            {
                BlazoredModal.Close(ModalResult.Ok(Specimen));
            }
        }

        protected void PopulateInventoryItemName()
        {
            if (SelectedLifeform != null && string.IsNullOrEmpty(Specimen.InventoryItem.Name))
            {
                Specimen.InventoryItem.Name = SelectedLifeform.ScientificName;
            }
        }

        protected async Task<IEnumerable<Origin>> FindOriginsAsync(string searchText)
        {
            var origins = (await ApiClient.FindOriginsAsync(searchText)).ToList();

            if (!string.IsNullOrEmpty(searchText))
            {
                origins.Add(new Origin { Name = searchText });
            }

            return origins;
        }

        protected async Task<IEnumerable<Lifeform>> FindLifeformsAsync(string searchText) => await ApiClient.FindLifeformsAsync(searchText);
    }
}