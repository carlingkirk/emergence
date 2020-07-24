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
    public class EditOriginComponent : ComponentBase
    {
        [Inject]
        protected IApiClient ApiClient { get; set; }
        [CascadingParameter]
        protected BlazoredModalInstance BlazoredModal { get; set; }
        [Parameter]
        public int Id { get; set; }
        public Origin Origin { get; set; }
        public string OriginUri { get; set; }
        public IEnumerable<OriginType> OriginTypes => Enum.GetValues(typeof(OriginType)).Cast<OriginType>();

        protected override async Task OnInitializedAsync()
        {
            if (Id > 0)
            {
                Origin = await ApiClient.GetOriginAsync(Id);
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
            Origin.DateModified = DateTime.UtcNow;

            Origin = await ApiClient.PutOriginAsync(Origin);

            if (BlazoredModal != null)
            {
                BlazoredModal.Close(ModalResult.Ok(Origin));
            }
        }
    }
}
