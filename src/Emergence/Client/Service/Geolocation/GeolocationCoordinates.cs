using System.Text.Json.Serialization;

namespace Emergence.Service.Geolocation
{
    /// <summary>
    /// Represents the data from a location query
    /// </summary>
    public class GeolocationCoordinates
    { // all these properties look awful, but I can't pass any options to the deserialiser in the js -> .net interop (and I really like my PascalCasing)
        /// <summary>
        /// The device's latitude
        /// </summary>
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        /// <summary>
        /// The device's longitude
        /// </summary>
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        /// <summary>
        /// The device's altitude
        /// </summary>
        [JsonPropertyName("altitude")]
        public double? Altitude { get; set; }
        /// <summary>
        /// The accuracy of the retrieved latitude and longitude, expressed in meters
        /// </summary>
        [JsonPropertyName("accuracy")]
        public double Accuracy { get; set; }

        /// <summary>
        /// The accuracy of the retrieved altitude, expressed in meters
        /// </summary>
        /// <remarks>
        /// May be null
        /// </remarks>
        [JsonPropertyName("altitudeAccuracy")]
        public double? AltitudeAccuracy { get; set; }

        /// <summary>
        /// The direction towards which the device is facing, in clockwise degrees from true north
        /// </summary>
        /// <remarks>
        /// May be null
        /// </remarks>
        [JsonPropertyName("heading")]
        public double? Heading { get; set; }

        /// <summary>
        /// The device's velocity in meters per second
        /// </summary>
        /// <remarks>
        /// May be null
        /// </remarks>
        [JsonPropertyName("speed")]
        public double? Speed { get; set; }

        /// <summary>
        /// Returns a the latitude and longitude in a string
        /// </summary>
        /// <returns>A string with a latitude and longitude</returns>
        public override string ToString() => $"{Latitude:f} {Longitude:f}";
    }
}
