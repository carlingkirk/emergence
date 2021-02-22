using System;

namespace Emergence.Service.Geolocation
{
    /// <summary>
    /// Wraps a <see cref="GeolocationPositionError"/> object for use in events
    /// </summary>
    public class PositionErrorEventArgs : EventArgs
    {
        /// <summary>
        /// The <see cref="GeolocationPositionError"/> object that is wrapped
        /// </summary>
        public GeolocationPositionError Error { get; set; }
    }
}
