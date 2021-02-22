using System.Text.Json.Serialization;

namespace Emergence.Service.Geolocation
{
    /// <summary>
    /// A class that contains options for retrieving locations
    /// </summary>
    public class PositionOptions
    {
        /// <summary>
        /// Indicates whether to sacrifice performance for accuracy
        /// </summary>
        /// <remarks>
        /// May result in longer response times
        /// </remarks>
        [JsonPropertyName("enableHighAccuracy")]
        public bool EnableHighAccuracy { get; set; } = false;

        /// <summary>
        /// The longest time the device may take to retrieve the location, in milliseconds
        /// </summary>
        [JsonPropertyName("timeout")]
        public long Timeout { get; set; } = long.MaxValue;

        /// <summary>
        /// The oldest a cached position may be
        /// </summary>
        [JsonPropertyName("maximumAge")]
        public long MaximumAge { get; set; } = 0;

        /// <summary>
        /// Returns a string representation of the options
        /// </summary>
        /// <returns>A human-readable string</returns>
        public override string ToString() => $"EnableHighAccuracy: {EnableHighAccuracy}, Timeout: {Timeout}, MaximumAge: {MaximumAge}";
    }
}
