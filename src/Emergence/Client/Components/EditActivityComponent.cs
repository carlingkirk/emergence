using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.Modal;
using Blazored.Modal.Services;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class EditActivityComponent : ActivityComponent
    {
        public bool CreateNewSpecimen { get; set; }
        [CascadingParameter]
        protected BlazoredModalInstance BlazoredModal { get; set; }

        public EditActivityComponent()
        {
            CreateNewSpecimen = true;
        }

        protected override async Task OnInitializedAsync() => await base.OnInitializedAsync();

        protected async Task SaveActivityAsync()
        {
            if (Activity.ActivityId == 0)
            {
                Activity.DateCreated = DateTime.UtcNow;
            }
            else
            {
                Activity.DateModified = DateTime.UtcNow;
            }

            if (UploadedPhotos.Any())
            {
                Activity.Photos = UploadedPhotos;
            }

            if (SelectedSpecimen != null)
            {
                Activity.Specimen = SelectedSpecimen;
            }

            if (CreateNewSpecimen && Activity.Quantity.HasValue)
            {
                var stage = SpecimenStage.Unknown;
                if (Activity.ActivityType == ActivityType.Stratification)
                {
                    stage = SpecimenStage.Stratification;
                }
                else if (Activity.ActivityType == ActivityType.PlantInGround)
                {
                    stage = SpecimenStage.InGround;
                }
                else if (Activity.ActivityType == ActivityType.Germination)
                {
                    stage = SpecimenStage.Germination;
                }

                var newSpecimen = new Specimen
                {
                    SpecimenStage = stage,
                    Lifeform = SelectedSpecimen.Lifeform,
                    InventoryItem = new InventoryItem
                    {
                        Name = SelectedSpecimen.InventoryItem.Name,
                        ItemType = ItemType.Specimen,
                        Quantity = Activity.Quantity.Value,
                        Status = SelectedSpecimen.InventoryItem.Status,
                        Inventory = SelectedSpecimen.InventoryItem.Inventory,
                        Origin = SelectedSpecimen.InventoryItem.Origin,
                        DateAcquired = Activity.DateOccured,
                        DateCreated = DateTime.UtcNow,
                        CreatedBy = UserId
                    },
                    CreatedBy = UserId,
                    DateCreated = DateTime.UtcNow
                };
                newSpecimen = await ApiClient.PutSpecimenAsync(newSpecimen);
                Activity.Specimen = newSpecimen;

                SelectedSpecimen.InventoryItem.Quantity -= Activity.Quantity.Value;
                await ApiClient.PutSpecimenAsync(SelectedSpecimen);
            }

            Activity = await ApiClient.PutActivityAsync(Activity);
            Id = Activity.ActivityId;

            if (BlazoredModal != null)
            {
                await BlazoredModal.Close(ModalResult.Ok(Activity));
            }
            else
            {
                await IsEditingChanged.InvokeAsync(false);
                await IsItemLoadedChanged.InvokeAsync(false);
            }
        }

        protected async Task<IEnumerable<Specimen>> FindSpecimensAsync(string searchText)
        {
            var specimenResult = await ApiClient.FindSpecimensAsync(new FindParams
            {
                SearchText = searchText,
                Skip = 0,
                Take = 10,
                SortBy = "ScientificName",
                SortDirection = SortDirection.Ascending
            });

            var specimens = specimenResult.Results.ToList();

            var result = await ApiClient.FindLifeformsAsync(new FindParams
            {
                SearchText = searchText,
                Skip = 0,
                Take = 3,
                SortBy = "ScientificName",
                SortDirection = SortDirection.Ascending
            });

            foreach (var lifeform in result.Results)
            {
                specimens.Add(new Specimen { Lifeform = lifeform, InventoryItem = new InventoryItem { Inventory = new Inventory { CreatedBy = UserId } } });
            }

            return specimens;
        }

        protected void PopulateActivityName()
        {
            if (Activity.ActivityType != ActivityType.Custom && SelectedSpecimen != null)
            {
                Activity.Name = Activity.ActivityType.ToFriendlyName() + ": " + SelectedSpecimen.Lifeform.ScientificName;
            }
        }
    }
}
