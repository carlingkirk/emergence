using System;
using Emergence.Functions.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Emergence.Functions.Startup))]
namespace Emergence.Functions
{
    internal class Startup : FunctionsStartup
    {
        public Startup()
        {
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var localRoot = Environment.GetEnvironmentVariable("AzureWebJobsScriptRoot");
            var azureRoot = $"{Environment.GetEnvironmentVariable("HOME")}/site/wwwroot";

            var actualRoot = localRoot ?? azureRoot;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(actualRoot)
                .AddJsonFile($"local.appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services.AddLogging();
            builder.Services.AddSingleton<IConfiguration>(configuration);
            builder.Services.AddSingleton<IConfigurationService, FunctionConfigurationService>();
            builder.Services.AddTransient<IBlobService, BlobService>();
            builder.Services.AddTransient<IPhotoService, PhotoService>();
        }
    }
}
