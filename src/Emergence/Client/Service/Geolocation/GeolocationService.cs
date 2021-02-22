using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Emergence.Service.Geolocation
{
    /// <summary>
    /// The main entry point of the API
    /// </summary>
    public class GeolocationService : IGeolocationService
    {
        /// <inheritdoc/>
        public event EventHandler<PositionChangedEventArgs> PositionChanged;

        /// <inheritdoc/>
        public event EventHandler<PositionErrorEventArgs> PositionError;

        private readonly IJSRuntime jsRuntime;
        private readonly Dictionary<long, List<IDisposable>> disposablesByWatchId = new Dictionary<long, List<IDisposable>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="GeolocationService"/> class, and requests objects through dependency injection
        /// </summary>
        /// <param name="jSRuntime">An <see cref="IJSRuntime"/> object</param>
        public GeolocationService(IJSRuntime jSRuntime)
        {
            jsRuntime = jSRuntime ?? throw new ArgumentNullException(nameof(jSRuntime));
        }

        /// <summary>
        /// Destructs the object and disposes of remaining <see cref="IDisposable"/>s
        /// </summary>
        ~GeolocationService()
        {
            foreach (var (_, value) in disposablesByWatchId)
            {
                value.ForEach(val => val.Dispose());
            }
        }

        /// <inheritdoc/>
        /// <exception cref="GeolocationPositionException">Thrown when an error is encountered in the browser API</exception>
        public async Task<GeolocationPosition> GetCurrentPositionAsync(PositionOptions options = null)
        {
            var tcs = new TaskCompletionSource<GeolocationPosition>();

            await GetCurrentPositionAsync((GeolocationPosition pos) => tcs.SetResult(pos),
                                          (GeolocationPositionError err) => tcs.SetException(new GeolocationPositionException(err)),
                                          options);

            return await tcs.Task;
        }

        /// <inheritdoc/>
        public async Task GetCurrentPositionAsync(Action<GeolocationPosition> success, Action<GeolocationPositionError> error = null, PositionOptions options = null)
        {
            var successJsAction = new JSAction<GeolocationPosition>(success);
            var errorJsAction = new JSAction<GeolocationPositionError>(error);

            var successRef = DotNetObjectReference.Create(successJsAction);
            var errorRef = DotNetObjectReference.Create(errorJsAction);

            await InjectJSHelper();

            await jsRuntime.InvokeVoidAsync("GeolocationBlazor.GetCurrentPosition", successRef, errorRef, options);
        }

        /// <inheritdoc/>
        public Task AttachEvents() =>
            WatchPositionAsync(pos => OnPositionChanged(new PositionChangedEventArgs { Position = pos }),
                               err => OnPositionError(new PositionErrorEventArgs { Error = err }));

        /// <inheritdoc/>
        public Task ClearWatchAsync(long id)
        {
            disposablesByWatchId.Remove(id, out var disposables);
            disposables?.ForEach(disposable => disposable.Dispose());

            return jsRuntime.InvokeVoidAsync("navigator.geolocation.clearWatch", id).AsTask();
        }

        /// <inheritdoc/>
        public async Task<long> WatchPositionAsync(Action<GeolocationPosition> success, Action<GeolocationPositionError> error = null, PositionOptions options = null)
        {
            var successJsAction = new JSAction<GeolocationPosition>(success);
            var errorJsAction = new JSAction<GeolocationPositionError>(error);

            var successRef = DotNetObjectReference.Create(successJsAction);
            var errorRef = DotNetObjectReference.Create(errorJsAction);

            await InjectJSHelper();

            var id = await jsRuntime.InvokeAsync<long>("GeolocationBlazor.WatchPosition", successRef, errorRef, options).AsTask();

            disposablesByWatchId.Add(id, new List<IDisposable> { successRef, errorRef });

            return id;
        }
        /// <summary>
        /// A protected method that is called when <see cref="PositionChanged"/> should fire
        /// </summary>
        /// <param name="eventArgs">A <see cref="PositionChangedEventArgs"/> object representing the event arguments</param>
        protected void OnPositionChanged(PositionChangedEventArgs eventArgs) => PositionChanged?.Invoke(this, eventArgs);

        /// <summary>
        /// A protected method that is called when <see cref="PositionError"/> should fire
        /// </summary>
        /// <param name="eventArgs">A <see cref="PositionErrorEventArgs"/> object representing the event arguments</param>
        protected void OnPositionError(PositionErrorEventArgs eventArgs) => PositionError?.Invoke(this, eventArgs);

        /// <summary>
        /// Creates a JavaScript object with helper methods
        /// </summary>
        protected ValueTask InjectJSHelper()
            => jsRuntime.InvokeVoidAsync("eval",// class instance to POJO conversion (ClassToObject) is neccesary, otherwise serialisation does not work
                @"var GeolocationBlazor = {
                    GetCurrentPosition:
                        function(successActionRef, errorActionRef, options) {
                            navigator.geolocation.getCurrentPosition(
                                function(position) {
                                    successActionRef?.invokeMethodAsync('Invoke',{""coords"":GeolocationBlazor.ClassToObject(position.coords), ""timestamp"":position.timestamp});
                                    successActionRef?.dispose();
                                },
                                function(error) {
                                    errorActionRef?.invokeMethodAsync('Invoke', GeolocationBlazor.ClassToObject(error));
                                    errorActionRef?.dispose();
                                },
                                options
                            );
                        },
                    WatchPosition:
                        function(successActionRef, errorActionRef, options) {
                            return navigator.geolocation.watchPosition(
                                function(position) {
                                    successActionRef?.invokeMethodAsync('Invoke',{""coords"":GeolocationBlazor.ClassToObject(position.coords), ""timestamp"":position.timestamp});
                                },
                                function(error) {
                                    errorActionRef?.invokeMethodAsync('Invoke', GeolocationBlazor.ClassToObject(error));
                                },
                                options
                            );
                        },
                    ClassToObject:
                        function(theClass) {
                            var originalClass = theClass || {};
                            var keys = Object.getOwnPropertyNames(Object.getPrototypeOf(originalClass));
                            return keys.reduce((classAsObj, key) => {
                                classAsObj[key] = originalClass[key];
                                return classAsObj;
                            }, {});
                        }
                    }");
    }
}
