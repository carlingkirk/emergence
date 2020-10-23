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
    }
}