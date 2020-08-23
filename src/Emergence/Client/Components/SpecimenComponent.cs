using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class SpecimenComponent : ViewerComponent
    {
        [Parameter]
        public Specimen Specimen { get; set; }
        public Origin SelectedOrigin { get; set; }
        public Lifeform SelectedLifeform { get; set; }
        public IList<Photo> UploadedPhotos { get; set; }
        public IEnumerable<SpecimenStage> SpecimenStages => Enum.GetValues(typeof(SpecimenStage)).Cast<SpecimenStage>();
        public IEnumerable<ItemType> ItemTypes => Enum.GetValues(typeof(ItemType)).Cast<ItemType>();
        public IEnumerable<Status> Statuses => Enum.GetValues(typeof(Status)).Cast<Status>();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (Id > 0 || Specimen != null)
            {
                Specimen ??= await ApiClient.GetSpecimenAsync(Id);
                SelectedOrigin = Specimen.InventoryItem.Origin ?? null;
                SelectedLifeform = Specimen.Lifeform;

                if (Specimen.Lifeform != null)
                {
                    Specimen.InventoryItem.Name = Specimen.Lifeform.ScientificName;
                }

                if (Specimen.Photos != null && Specimen.Photos.Any())
                {
                    UploadedPhotos = Specimen.Photos.ToList();
                }
                else
                {
                    UploadedPhotos = new List<Photo>();
                }

                if (!string.IsNullOrEmpty(UserId) && Specimen.InventoryItem.Inventory.UserId == UserId)
                {
                    IsEditable = true;
                }
            }
            else if (Specimen == null)
            {
                IsEditable = true;
                Specimen = new Specimen
                {
                    Lifeform = new Lifeform(),
                    InventoryItem = new InventoryItem()
                };
                UploadedPhotos = new List<Photo>();
            }
        }
    }
}
