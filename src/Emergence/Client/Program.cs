using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.Modal;
using Emergence.Client.Common;
using Emergence.Service.Geolocation;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Emergence.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Emergence.BaseAPI"));

            builder.Services.AddHttpClient("Emergence.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddHttpClient("Emergence.AnonymousAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            builder.Services.AddApiAuthorization(options =>
            {
                options.UserOptions.NameClaim = "name";
                options.UserOptions.RoleClaim = "role";
            }).AddAccountClaimsPrincipalFactory<CustomUserFactory>();

            builder.Services.AddBlazoredModal();

            // Client services
            builder.Services.AddTransient<IModalServiceClient, ModalServiceClient>();
            builder.Services.AddTransient<IGeolocationService, GeolocationService>();

            await builder.Build().RunAsync();
        }
    }
}
