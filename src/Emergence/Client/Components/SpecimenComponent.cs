using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data.Shared;
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
        public IEnumerable<ItemStatus> Statuses => Enum.GetValues(typeof(ItemStatus)).Cast<ItemStatus>();

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
            }
            else if (Specimen == null)
            {
                IsEditing = true;
                Specimen = new Specimen
                {
                    Lifeform = new Lifeform(),
                    InventoryItem = new InventoryItem { Inventory = new Inventory { CreatedBy = UserId, OwnerId = UserId, DateCreated = DateTime.UtcNow } }
                };
                UploadedPhotos = new List<Photo>();
            }

            if (!string.IsNullOrEmpty(UserId) && Specimen.InventoryItem.Inventory.CreatedBy == UserId)
            {
                IsEditable = true;
            }
        }
    }
}
