using Emergence.Data.Shared;
using Emergence.Service.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Emergence.Service
{
    public class ConfigurationService : IConfigurationService
    {
        public AppConfiguration Settings { get; set; }

        public ConfigurationService(IConfiguration configuration)
        {
            Settings = configuration.GetSection("App").Get<AppConfiguration>();
        }
    }
}
