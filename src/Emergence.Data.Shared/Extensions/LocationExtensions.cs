using System;
using Emergence.Data.Shared.Stores;

namespace Emergence.Data.Shared.Extensions
{
    public static class LocationExtensions
    {
        public static Models.Location AsModel(this Location source) => new Models.Location
        {
            LocationId = source.Id,
            Latitude = source.Latitude,
            Longitude = source.Longitude,
            Altitude = source.Altitude,
            AddressLine1 = source.AddressLine1,
            AddressLine2 = source.AddressLine2,
            City = source.City,
            StateOrProvince = source.StateOrProvince,
            PostalCode = source.PostalCode,
            Country = source.Country,
            DateCreated = source.DateCreated,
            DateModified = source.DateModified
        };

        public static Location AsStore(this Models.Location source) => new Location
        {
            Id = source.LocationId,
            Latitude = source.Latitude,
            Longitude = source.Longitude,
            Altitude = source.Altitude,
            AddressLine1 = source.AddressLine1,
            AddressLine2 = source.AddressLine2,
            City = source.City,
            StateOrProvince = source.StateOrProvince,
            PostalCode = source.PostalCode,
            Country = source.Country,
            DateCreated = source.DateCreated ?? DateTime.UtcNow,
            DateModified = source.DateModified
        };
    }
}
