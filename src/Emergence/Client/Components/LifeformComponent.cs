using System.Threading.Tasks;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class LifeformComponent : ViewerComponent<Lifeform>
    {
        [Parameter]
        public Lifeform Lifeform { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (Id > 0 || Lifeform != null)
            {
                Lifeform ??= await ApiClient.GetLifeformAsync(Id);
            }
        }
    }
}
