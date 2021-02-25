using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;
using Emergence.Data.Shared.Models.Places;
using GoogleApi;
using GoogleApi.Entities.Maps.Geocoding.Address.Request;
using GoogleApi.Entities.Places.Details.Request;
using GoogleApi.Entities.Places.Details.Response;
using GoogleApi.Entities.Places.Search.Find.Request;
using GoogleApi.Entities.Places.Search.Find.Response;
using Microsoft.Extensions.Configuration;

namespace Emergence.Service
{
    public interface IPlaceService
    {
        Task<IEnumerable<Candidate>> SearchAsync(string searchText);
        Task<DetailsResult> GetPlaceDetailAsync(string placeId);
        Task<IEnumerable<Place>> GetGeocodesAsync(string state, string region = null);
    }

    public class PlaceService : IPlaceService
    {
        private readonly string ApiKey;
        private readonly string ClientId;

        public PlaceService(IConfiguration configuration)
        {
            ApiKey = configuration["GoogleApiKey"];
            ClientId = configuration["GoogleClientId"];
        }

        public async Task<IEnumerable<Candidate>> SearchAsync(string searchText)
        {
            var request = await GooglePlaces.FindSearch.QueryAsync(new PlacesFindSearchRequest
            {
                ClientId = ClientId,
                Key = ApiKey,
                Input = searchText
            });

            return request.Candidates;
        }

        public async Task<DetailsResult> GetPlaceDetailAsync(string placeId)
        {
            var request = await GooglePlaces.Details.QueryAsync(new PlacesDetailsRequest
            {
                ClientId = ClientId,
                Key = ApiKey,
                PlaceId = placeId
            });

            return request.Result;
        }

        public async Task<IEnumerable<Place>> GetGeocodesAsync(string address, string region = null)
        {
            var request = new AddressGeocodeRequest
            {
                ClientId = ClientId,
                Key = ApiKey,
                Address = address,
                Region = region
            };

            var result = await GoogleMaps.AddressGeocode.QueryAsync(request);

            return result.Results.Select(r => new Place
            {
                PlaceId = r.PlaceId,
                AddressComponents = r.AddressComponents.Select(ac => new AddressComponent
                {
                    ShortName = ac.ShortName,
                    LongName = ac.LongName,
                    Types = ac.Types.Select(t => (AddressComponentType)t)
                }),
                FormattedAddress = r.FormattedAddress,
                PartialMatch = r.PartialMatch,
                Types = r.Types.Select(t => (PlaceLocationType)t),
                Geometry = new Geometry
                {
                    Bounds = new ViewPort
                    {
                        Northeast = new PlaceLocation { Address = r.Geometry.Bounds.NorthEast.Address, Latitude = r.Geometry.Bounds.NorthEast.Latitude, Longitude = r.Geometry.Bounds.NorthEast.Longitude },
                        Southwest = new PlaceLocation { Address = r.Geometry.Bounds.SouthWest.Address, Latitude = r.Geometry.Bounds.SouthWest.Latitude, Longitude = r.Geometry.Bounds.SouthWest.Longitude }
                    },
                    Location = new PlaceLocation { Address = r.Geometry.Location.Address, Latitude = r.Geometry.Location.Latitude, Longitude = r.Geometry.Location.Longitude },
                    LocationType = (GeometryLocationType)r.Geometry.LocationType,
                    ViewPort = new ViewPort
                    {
                        Northeast = new PlaceLocation { Address = r.Geometry.ViewPort.NorthEast.Address, Latitude = r.Geometry.ViewPort.NorthEast.Latitude, Longitude = r.Geometry.ViewPort.NorthEast.Longitude },
                        Southwest = new PlaceLocation { Address = r.Geometry.ViewPort.SouthWest.Address, Latitude = r.Geometry.ViewPort.SouthWest.Latitude, Longitude = r.Geometry.ViewPort.SouthWest.Longitude }
                    }
                },
                PlusCode = new PlusCode { LocalCode = r.PlusCode.LocalCode, GlobalCode = r.PlusCode.GlobalCode },
                PostcodeLocalities = r.PostcodeLocalities
            });
        }
    }
}
