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
        [CascadingParameter]
        protected BlazoredModalInstance BlazoredModal { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

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

            Activity = await ApiClient.PutActivityAsync(Activity);
            Id = Activity.ActivityId;

            if (BlazoredModal != null)
            {
                await BlazoredModal.Close(ModalResult.Ok(Activity));
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

            var lifeforms = await ApiClient.FindLifeformsAsync(new FindParams
            {
                SearchText = searchText,
                Skip = 0,
                Take = 3,
                SortBy = "ScientificName",
                SortDirection = SortDirection.Ascending
            });

            foreach (var lifeform in lifeforms)
            {
                specimens.Add(new Specimen { Lifeform = lifeform, InventoryItem = new InventoryItem() });
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
