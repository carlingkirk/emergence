using System.Text.Json.Serialization;

namespace Emergence.Service.Geolocation
{
    /// <summary>
    /// A class that contains info about an encountered error while querying a device's location
    /// </summary>
    public class GeolocationPositionError
    {
        /// <summary>
        /// An error code representing the encountered error
        /// </summary>
        /// <remarks>
        /// Three error codes are possible:
        /// <list type="table">
        /// <listheader>
        /// <item>Value</item>
        /// <item>Description</item>
        /// </listheader>
        /// <item>
        /// <term>1</term>
        /// <term>The acquisition of the geolocation information failed because the page didn't have the permission to do it.</term>
        /// </item>
        /// <item>
        /// <term>2</term>
        /// <term>The acquisition of the geolocation failed because at least one internal source of position returned an internal error.</term>
        /// </item>
        /// <item>
        /// <term>3</term>
        /// <term>The time allowed to acquire the geolocation, defined by <see cref="PositionOptions.Timeout"/> information was reached before the information was obtained.</term>
        /// </item>
        /// </list>
        /// </remarks>
        [JsonPropertyName("code")]
        public ushort Code { get; set; }

        /// <summary>
        /// A human-readable string representation of the error
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }

        /// <summary>
        /// Returns the <see cref="Message"/> property
        /// </summary>
        /// <returns>A human-readable string</returns>
        public override string ToString() => Message ?? "";
    }
}
