using System.Text.Json.Serialization;

namespace Emergence.Service.Geolocation
{
    /// <summary>
    /// A class that contains a timestamp and a <see cref="GeolocationCoordinates"/> object
    /// </summary>
    public class GeolocationPosition
    {
        /// <summary>
        /// A <see cref="GeolocationCoordinates"/> object that contains the location query result
        /// </summary>
        [JsonPropertyName("coords")]
        public GeolocationCoordinates Coords { get; set; }

        /// <summary>
        /// The time of retrieval, unix time expressed in milliseconds
        /// </summary>
        [JsonPropertyName("timestamp")]
        public ulong Timestamp { get; set; }

        /// <summary>
        /// Returns a string representation of the <see cref="Coords"/> property
        /// </summary>
        /// <returns>A string that represents a <see cref="GeolocationCoordinates"/> object</returns>
        public override string ToString() => Coords?.ToString() ?? new GeolocationCoordinates().ToString();
    }
}
