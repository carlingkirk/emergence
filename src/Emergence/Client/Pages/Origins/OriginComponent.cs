using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class OriginComponent : ViewerComponent<Origin>
    {
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
                IsEditing = true;
                Origin = new Origin
                {
                    CreatedBy = UserId,
                    DateCreated = DateTime.Now,
                    ParentOrigin = null,
                    Location = new Location()
                };
            }
        }

        protected async Task RemoveOrigin()
        {
            var result = await ApiClient.RemoveOriginAsync(Origin);
            if (result)
            {
                Origin = null;

                await RefreshListAsync();
                await UnloadItem();
            }
        }
    }
}
