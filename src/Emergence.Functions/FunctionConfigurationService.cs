using Emergence.Functions.Services;
using Microsoft.Extensions.Configuration;

namespace Emergence.Functions
{
    public class FunctionConfigurationService : IConfigurationService
    {
        public AppConfiguration Settings { get; set; }

        public FunctionConfigurationService(IConfiguration configuration)
        {
            Settings = new AppConfiguration
            {
                BlobStorageRoot = configuration["BlobStorageRoot"]
            };
        }
    }
}
