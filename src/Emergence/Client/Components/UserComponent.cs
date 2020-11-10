using System.Threading.Tasks;
using Emergence.Client.Common;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class UserComponent : EmergenceComponent
    {
        [Parameter]
        public User User { get; set; }
        [Parameter]
        public int Id { get; set; }
        [Parameter]
        public string Name { get; set; }
        [Parameter]
        public bool IsModal { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (Id > 0)
            {
                User ??= await ApiClient.GetUserAsync(Id);
            }
            else if (!string.IsNullOrEmpty(Name))
            {
                User ??= await ApiClient.GetUserByNameAsync(Name);
            }
        }

        protected async Task AddContactRequestAsync()
        {
            var userContactRequest = await ApiClient.AddContactRequestAsync(new UserContactRequest { UserId = User.Id });
            if (userContactRequest != null)
            {
                User.IsViewerContactRequested = true;
            }
        }
    }
}
