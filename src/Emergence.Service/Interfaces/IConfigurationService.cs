using Emergence.Data.Shared;

namespace Emergence.Service.Interfaces
{
    public interface IConfigurationService
    {
        public AppConfiguration Settings { get; set; }
    }
}
