using Microsoft.Extensions.Configuration;

namespace Emergence.Functions.Services
{
    public interface IConfigurationService
    {
        public AppConfiguration Settings { get; set; }
    }

    public class ConfigurationService : IConfigurationService
    {
        public AppConfiguration Settings { get; set; }

        public ConfigurationService(IConfiguration configuration)
        {
            Settings = configuration.GetSection("App").Get<AppConfiguration>();
        }
    }

    public class AppConfiguration
    {
        public string BlobStorageRoot { get; set; }
    }
}
