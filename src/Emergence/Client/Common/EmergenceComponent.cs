using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Emergence.Client.Common
{
    public class EmergenceComponent : ComponentBase
    {
        [Inject]
        protected IHttpClientFactory HttpClientFactory { get; set; }
        protected IApiClient ApiClient { get; set; }
        [CascadingParameter]
        protected Task<AuthenticationState> AuthenticationStateTask { get; set; }
        protected string UserId { get; set; }
        protected bool IsAuthenticated { get; set; }
        protected override async Task OnInitializedAsync()
        {
            if (AuthenticationStateTask != null)
            {
                var state = await AuthenticationStateTask;
                if (!state.User.Identity.IsAuthenticated)
                {
                    IsAuthenticated = false;
                    ApiClient = new ApiClient(HttpClientFactory.CreateClient("Emergence.AnonymousAPI"));
                }
                else
                {
                    IsAuthenticated = true;
                    ApiClient = new ApiClient(HttpClientFactory.CreateClient("Emergence.ServerAPI"));
                    UserId = state.User.FindFirst(c => c.Type == "sub")?.Value;
                }
            }
        }
    }
}
