using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Emergence.Client.Server.Areas.Identity.IdentityHostingStartup))]
namespace Emergence.Client.Server.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
