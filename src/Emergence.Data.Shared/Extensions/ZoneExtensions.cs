using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Extensions
{
    public static class ZoneExtensions
    {
        public static Search.Models.Zone AsSearchModel(this Zone source) => new Search.Models.Zone
        {
            Id = source.Id,
            Name = source.Name,
            Notes = source.Notes
        };
    }
}
