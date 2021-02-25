using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Client.Common;
using Emergence.Client.Service.Geolocation;
using Emergence.Data.Shared.Models;
using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Shared
{
    public class Map : EmergenceComponent
    {
        [Inject]
        private IGeolocationService GeolocationService { get; set; }
        protected GoogleMap GoogleMap { get; set; }
        protected MapOptions MapOptions { get; set; }
        protected Marker Marker { get; set; }
        protected Dictionary<Location, Marker> Markers { get; set; }
        protected string SearchText { get; set; }
        protected GeolocationPosition CurrentPosition { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Markers = new Dictionary<Location, Marker>();
            CurrentPosition = await GeolocationService.GetCurrentPositionAsync();

            MapOptions = new MapOptions()
            {
                Zoom = 13,
                Center = new LatLngLiteral()
                {
                    Lat = CurrentPosition.Coords.Latitude,
                    Lng = CurrentPosition.Coords.Longitude
                },
                MapTypeId = MapTypeId.Terrain
            };
        }

        protected async Task OnAfterInitAsync()
        {
            Marker = await Marker.CreateAsync(GoogleMap.JsRuntime, new MarkerOptions
            {
                Position = await GoogleMap.InteropObject.GetCenter(),
                Map = GoogleMap.InteropObject,
                Clickable = true,
                Draggable = true
            });

            await GoogleMap.InteropObject.AddListener<MouseEvent>("click", async (e) => await OnClick(e));
        }

        protected async Task OnClick(MouseEvent e)
        {
            var marker = await Marker.CreateAsync(GoogleMap.JsRuntime, new MarkerOptions
            {
                Position = e.LatLng,
                Map = GoogleMap.InteropObject,
                Clickable = true,
                Draggable = true
            });

            Markers.Add(new Location { Latitude = e.LatLng.Lat, Longitude = e.LatLng.Lng }, marker);

            StateHasChanged();

            await e.Stop();
        }

        protected async Task SearchAsync()
        {
            var region = new System.Globalization.RegionInfo(System.Globalization.CultureInfo.CurrentCulture.LCID);
            var places = await ApiClient.GetGeocodesAsync(SearchText, region.Name);

            foreach (var place in places)
            {
                Marker = await Marker.CreateAsync(GoogleMap.JsRuntime, new MarkerOptions
                {
                    Position = new LatLngLiteral { Lat = place.Geometry.Location.Latitude, Lng = place.Geometry.Location.Longitude },
                    Map = GoogleMap.InteropObject,
                    Clickable = true,
                    Draggable = true
                });

                Markers.Add(new Location
                {
                    Latitude = place.Geometry.Location.Latitude,
                    Longitude = place.Geometry.Location.Longitude
                }, Marker);
            }
        }
    }
}
