using System;
using System.Threading.Tasks;

namespace Emergence.Service.Geolocation
{
    /// <summary>
    /// An interface that defines a Geolocation Service
    /// </summary>
    public interface IGeolocationService
    {
        /// <summary>
        /// Gets the device's position
        /// </summary>
        /// <param name="options">Optional <see cref="PositionOptions"/> object</param>
        /// <returns>A <see cref="GeolocationPosition"/> object with the device's current location and the retrieval timestamp</returns>
        Task<GeolocationPosition> GetCurrentPositionAsync(PositionOptions options = null);

        /// <summary>
        /// Takes two delegates that will be called once, either when the location is acquired, or when there is an error
        /// </summary>
        /// <remarks>
        /// When there is an error, the success delegate will not be called
        /// </remarks>
        /// <param name="success">A delegate that takes a <see cref="GeolocationPosition"/> object, and returns nothing</param>
        /// <param name="error">A delegate that takes a <see cref="GeolocationPositionError"/> object, and returns nothing</param>
        /// <param name="options">An optional <see cref="PositionOptions"/> object</param>
        Task GetCurrentPositionAsync(Action<GeolocationPosition> success, Action<GeolocationPositionError> error = null, PositionOptions options = null);

        /// <summary>
        /// Registers two delegates that are called every time the browser detects a change in the device's location
        /// </summary>
        /// <remarks>
        /// May be called more than once with the same location
        /// </remarks>
        /// <param name="success">A delegate that takes a <see cref="GeolocationPosition"/> object, and returns nothing</param>
        /// <param name="error">A delegate that takes a <see cref="GeolocationPositionError"/> object, and returns nothing</param>
        /// <param name="options">An optional <see cref="PositionOptions"/> object</param>
        /// <returns>A unique identifier that may be used in <see cref="ClearWatchAsync(long)"/></returns>
        Task<long> WatchPositionAsync(Action<GeolocationPosition> success, Action<GeolocationPositionError> error = null, PositionOptions options = null);

        /// <summary>
        /// A function that unregisters a handler registered with <see cref="WatchPositionAsync(Action{GeolocationPosition}, Action{GeolocationPositionError}, PositionOptions)"/>
        /// </summary>
        /// <param name="id">A unique id, acquired from <see cref="WatchPositionAsync(Action{GeolocationPosition}, Action{GeolocationPositionError}, PositionOptions)"/></param>
        Task ClearWatchAsync(long id);

        /// <summary>
        /// A method that can be called to attach the <see cref="PositionChanged"/> and <see cref="PositionError"/> events
        /// </summary>
        Task AttachEvents();

        /// <summary>
        /// An event that is triggered when the device's position changes
        /// </summary>
        /// <remarks>
        /// For this event to fire at all, <see cref="AttachEvents"/> will first need to be called
        /// </remarks>
        event EventHandler<PositionChangedEventArgs> PositionChanged;

        /// <summary>
        /// An event that is triggered when an error is encountered querying the device's position
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        /// For this event to fire at all, <see cref="AttachEvents"/> will first need to be called
        /// </item>
        /// <item>
        /// This event is not fired when an error is encountered in another method, it is rather supposed to be used with <see cref="PositionChanged"/>
        /// </item>
        /// </list>
        /// </remarks>
        event EventHandler<PositionErrorEventArgs> PositionError;

    }
}
