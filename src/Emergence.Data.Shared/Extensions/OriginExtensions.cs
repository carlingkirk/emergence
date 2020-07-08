using System;
using System.Collections.Generic;
using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Extensions
{
    public static class OriginExtensions
    {
        public static Models.Origin AsModel(this Origin source, IEnumerable<Origin> items = null) => new Models.Origin
        {
            OriginId = source.Id,
            Name = source.Name,
            Description = source.Description,
            Type = Enum.Parse<Models.OriginType>(source.Type),
            Location = new Models.Location { Latitude = source.Latitude, Longitude = source.Longitude },
            Uri = source.Uri,
            Authors = source.Authors,
            ExternalId = source.ExternalId,
            AltExternalId = source.AltExternalId
        };

        public static Origin AsStore(this Models.Origin source) => new Origin
        {
            Id = source.OriginId
        };
    }
}
