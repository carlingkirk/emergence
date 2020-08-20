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
    public class EditOriginComponent : EmergenceComponent
    {
        [CascadingParameter]
        protected BlazoredModalInstance BlazoredModal { get; set; }
        [Parameter]
        public int Id { get; set; }
        [Parameter]
        public Origin Origin { get; set; }
        public Origin SelectedParentOrigin { get; set; }
        public string OriginUri { get; set; }
        public IEnumerable<OriginType> OriginTypes => Enum.GetValues(typeof(OriginType)).Cast<OriginType>();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (Id > 0 || Origin != null)
            {
                Origin ??= await ApiClient.GetOriginAsync(Id);
                Origin.Location ??= new Location();
                OriginUri = Origin.Uri?.ToString();

                if (Origin.ParentOrigin != null)
                {
                    SelectedParentOrigin = Origin.ParentOrigin;
                }
            }
            else
            {
                Origin = new Origin
                {
                    ParentOrigin = null,
                    Location = new Location()
                };
            }
        }

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

            Origin = await ApiClient.PutOriginAsync(Origin);

            if (BlazoredModal != null)
            {
                await BlazoredModal.Close(ModalResult.Ok(Origin));
            }
        }
    }
}
