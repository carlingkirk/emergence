using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class SpecimenComponent : ViewerComponent<Specimen>
    {
        [Parameter]
        public Specimen Specimen { get; set; }
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        public Origin SelectedOrigin { get; set; }
        public Lifeform SelectedLifeform { get; set; }
        public List<Photo> UploadedPhotos { get; set; }
        public IEnumerable<SpecimenStage> SpecimenStages => Enum.GetValues(typeof(SpecimenStage)).Cast<SpecimenStage>();
        public IEnumerable<ItemType> ItemTypes => Enum.GetValues(typeof(ItemType)).Cast<ItemType>();
        public IEnumerable<ItemStatus> Statuses => Enum.GetValues(typeof(ItemStatus)).Cast<ItemStatus>();
        protected bool IsOwner => !string.IsNullOrEmpty(UserId) && (Specimen?.CreatedBy == UserId);

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (Id > 0 || Specimen != null)
            {
                Specimen ??= await ApiClient.GetSpecimenAsync(Id);
                Specimen.Name = Specimen.InventoryItem.Name;
                Specimen.Quantity = Specimen.InventoryItem.Quantity;

                SelectedOrigin = Specimen.InventoryItem.Origin;
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
                    InventoryItem = new InventoryItem
                    {
                        ItemType = ItemType.Specimen,
                        Inventory = new Inventory
                        {
                            CreatedBy = UserId,
                            OwnerId = UserId,
                            DateCreated = DateTime.UtcNow
                        },
                        DateCreated = DateTime.UtcNow,
                        CreatedBy = UserId
                    },
                    CreatedBy = UserId
                };
                UploadedPhotos = new List<Photo>();
            }
        }

        protected async Task RemoveSpecimen()
        {
            var result = await ApiClient.RemoveSpecimenAsync(Specimen);
            if (result)
            {
                Specimen = null;

                await RefreshListAsync();
                await UnloadItem();
            }
        }

        protected UserMessage GetMessage() => new UserMessage
        {
            User = Specimen.InventoryItem.User,
            Subject = "Re: " + (Specimen.Lifeform?.CommonName ?? Specimen.Name) + " " + Specimen.Lifeform?.ScientificName,
            MessageBody = $"Regarding your {Specimen.Lifeform?.CommonName ?? Specimen.Name} at {NavigationManager.BaseUri + "specimen/" + Specimen.SpecimenId},\r\n"
        };
    }
}
