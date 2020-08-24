using System;
using System.Threading.Tasks;
using Blazored.Modal;
using Blazored.Modal.Services;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class EditOriginComponent : OriginComponent
    {
        [CascadingParameter]
        protected BlazoredModalInstance BlazoredModal { get; set; }

        protected async Task SaveOriginAsync()
        {
            if (Origin.OriginId == 0)
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

            if (SelectedParentOrigin != null)
            {
                Origin.ParentOrigin = SelectedParentOrigin;
            }

            Origin.UserId = UserId;
            if (Origin.Location != null)
            {
                if (Origin.Location.LocationId == 0)
                {
                    Origin.Location.DateCreated = DateTime.UtcNow;
                }

                Origin.Location.DateModified = DateTime.UtcNow;
            }
            else
            {
                Console.WriteLine("WHYYYYY");
            }
            Origin = await ApiClient.PutOriginAsync(Origin);

            if (BlazoredModal != null)
            {
                await BlazoredModal.Close(ModalResult.Ok(Origin));
            }
            else
            {
                await IsEditingChanged.InvokeAsync(false);
                await IsItemLoadedChanged.InvokeAsync(false);
            }
        }
    }
}
