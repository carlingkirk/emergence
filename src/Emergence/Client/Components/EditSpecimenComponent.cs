using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.Modal;
using Blazored.Modal.Services;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class EditSpecimenComponent : SpecimenComponent
    {
        [CascadingParameter]
        protected BlazoredModalInstance BlazoredModal { get; set; }

        protected async Task SaveSpecimenAsync()
        {
            if (Specimen.SpecimenId == 0)
            {
                Specimen.DateCreated = DateTime.UtcNow;
            }
            else
            {
                Specimen.DateModified = DateTime.UtcNow;
            }

            if (SelectedOrigin != null)
            {
                Specimen.InventoryItem.Origin = SelectedOrigin;
            }

            if (SelectedLifeform != null)
            {
                Specimen.Lifeform = SelectedLifeform;
            }

            if (UploadedPhotos.Any())
            {
                Specimen.Photos = UploadedPhotos;
            }

            Specimen = await ApiClient.PutSpecimenAsync(Specimen);

            if (BlazoredModal != null)
            {
                await BlazoredModal.Close(ModalResult.Ok(Specimen));
            }
            else
            {
                Specimen = null;
                await IsEditingChanged.InvokeAsync(false);
                await IsItemLoadedChanged.InvokeAsync(false);
            }
        }

        protected void PopulateInventoryItemName()
        {
            if (SelectedLifeform != null && string.IsNullOrEmpty(Specimen.InventoryItem.Name))
            {
                Specimen.InventoryItem.Name = SelectedLifeform.ScientificName;
            }
        }

        protected async Task<IEnumerable<Lifeform>> FindLifeformsAsync(string searchText) =>
            await ApiClient.FindLifeformsAsync(new FindParams
            {
                SearchText = searchText,
                Skip = 0,
                Take = 10,
                SortBy = "ScientificName",
                SortDirection = SortDirection.Ascending
            });
    }
}
