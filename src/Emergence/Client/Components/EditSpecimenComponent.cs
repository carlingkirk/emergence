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
        [Parameter]
        public Func<Task> Cancel { get; set; }

        protected async Task SaveSpecimenAsync()
        {
            var isNewSpecimen = Specimen.SpecimenId == 0;
            if (isNewSpecimen)
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

            Specimen.Photos = UploadedPhotos.Any() ? UploadedPhotos : null;

            Specimen = await ApiClient.PutSpecimenAsync(Specimen);

            if (BlazoredModal != null)
            {
                await BlazoredModal.Close(ModalResult.Ok(Specimen));
            }
            else
            {
                await CancelAsync(isNewSpecimen);
            }
        }

        protected async Task CancelAsync(bool isNewSpecimen = false)
        {
            if (Specimen.SpecimenId == 0 || isNewSpecimen)
            {
                await Cancel.Invoke();

                if (isNewSpecimen)
                {
                    await RefreshListAsync();
                }
            }
            else
            {
                await IsEditingChanged.InvokeAsync(false);
                await RefreshListAsync();
            }
        }

        protected void PopulateInventoryItemName()
        {
            if (SelectedLifeform != null && string.IsNullOrEmpty(Specimen.InventoryItem.Name))
            {
                Specimen.InventoryItem.Name = SelectedLifeform.ScientificName;
            }
        }

        protected async Task<IEnumerable<Lifeform>> FindLifeformsAsync(string searchText)
        {
            var result = await ApiClient.FindLifeformsAsync(new FindParams
            {
                SearchText = searchText,
                Skip = 0,
                Take = 10,
                SortBy = "ScientificName",
                SortDirection = SortDirection.Ascending
            });

            return result.Results;
        }
    }
}
