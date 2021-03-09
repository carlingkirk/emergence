using System;
using System.Threading.Tasks;
using Blazored.Modal;
using Blazored.Modal.Services;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class EditOriginComponent : OriginComponent
    {
        [CascadingParameter]
        protected BlazoredModalInstance BlazoredModal { get; set; }
        [Parameter]
        public Func<Task> Cancel { get; set; }

        protected async Task SaveOriginAsync()
        {
            var isNewOrigin = Origin.OriginId == 0;
            if (isNewOrigin)
            {
                Origin.DateCreated = DateTime.UtcNow;
            }
            else
            {
                Origin.DateModified = DateTime.UtcNow;
            }

            if (!string.IsNullOrEmpty(OriginUri))
            {
                Origin.Uri = new Uri(OriginUri);
            }

            Origin.ParentOrigin = SelectedParentOrigin;
            Origin.CreatedBy ??= UserId;
            Origin.ModifiedBy = UserId;

            if (Origin.Location != null)
            {
                if (Origin.Location.IsNotEmpty())
                {
                    if (Origin.Location.LocationId == 0)
                    {
                        Origin.Location.DateCreated = DateTime.UtcNow;
                    }
                    else
                    {
                        Origin.Location.DateModified = DateTime.UtcNow;
                    }
                }
                else
                {
                    Origin.Location = null;
                }
            }

            Origin = await ApiClient.PutOriginAsync(Origin);

            if (BlazoredModal != null)
            {
                await BlazoredModal.Close(ModalResult.Ok(Origin));
            }
            else
            {
                await CancelAsync(isNewOrigin);
            }
        }

        protected async Task CancelAsync(bool isNewOrigin = false)
        {
            if (Origin.OriginId == 0 || isNewOrigin)
            {
                if (isNewOrigin)
                {
                    await RefreshListAsync();
                }

                await Cancel.Invoke();
            }
            else
            {
                await IsEditingChanged.InvokeAsync(false);

                await RefreshListAsync();
            }
        }
    }
}
