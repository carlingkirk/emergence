using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;

namespace Emergence.Client.Common
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Origin>> FindOriginsAsync(string searchText, int? skip = 0, int? take = 10)
        {
            var origins = (await _httpClient.GetFromJsonAsync<IEnumerable<Origin>>($"/api/origin/find?search={searchText}&skip={skip}&take={take}")).ToList();

            origins.Add(new Origin { Name = searchText });

            return origins;
        }

        public async Task<IEnumerable<Lifeform>> FindLifeformsAsync(string searchText, int? skip = 0, int? take = 10)
        {
            var result = await _httpClient.GetAsync($"/api/lifeform/find?search={searchText}&skip={skip}&take={take}");

            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<IEnumerable<Lifeform>>();
            }
            else
            {
                var message = result.Content.ReadAsStringAsync();
                throw new Exception(result.StatusCode + ": " + message);
            }
        }

        public async Task<IEnumerable<Specimen>> FindSpecimensAsync(string searchText, int? skip = 0, int? take = 10)
        {
            var result = await _httpClient.GetAsync($"/api/specimen/find?search={searchText}&skip={skip}&take={take}");

            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<IEnumerable<Specimen>>();
            }
            else
            {
                var message = result.Content.ReadAsStringAsync();
                throw new Exception(result.StatusCode + ": " + message);
            }
        }

        public async Task<IEnumerable<Activity>> FindActivitiesAsync(string searchText, int? skip = 0, int? take = 10)
        {
            var result = await _httpClient.GetAsync($"/api/activity/find?search={searchText}&skip={skip}&take={take}");

            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<IEnumerable<Activity>>();
            }
            else
            {
                var message = result.Content.ReadAsStringAsync();
                throw new Exception(result.StatusCode + ": " + message);
            }
        }

        public async Task<IEnumerable<PlantInfo>> FindPlantInfosAsync(string searchText, int? skip = 0, int? take = 10)
        {
            var result = await _httpClient.GetAsync($"/api/plantinfo/find?search={searchText}&skip={skip}&take={take}");

            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<IEnumerable<PlantInfo>>();
            }
            else
            {
                var message = result.Content.ReadAsStringAsync();
                throw new Exception(result.StatusCode + ": " + message);
            }
        }

        public async Task<Specimen> GetSpecimenAsync(int id)
        {
            var result = await _httpClient.GetAsync($"/api/specimen/{id}");

            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<Specimen>();
            }
            else
            {
                var message = result.Content.ReadAsStringAsync();
                throw new Exception(result.StatusCode + ": " + message);
            }
        }

        public async Task<Specimen> PutSpecimenAsync(Specimen specimen)
        {
            var result = await _httpClient.PutAsJsonAsync("/api/specimen", specimen);

            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<Specimen>();
            }

            return null;
        }

        public async Task<PlantInfo> GetPlantInfoAsync(int id)
        {
            var result = await _httpClient.GetAsync($"/api/plantinfo/{id}");

            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<PlantInfo>();
            }
            else
            {
                var message = result.Content.ReadAsStringAsync();
                throw new Exception(result.StatusCode + ": " + message);
            }
        }

        public async Task<PlantInfo> PutPlantInfoAsync(PlantInfo plantInfo)
        {
            var result = await _httpClient.PutAsJsonAsync("/api/plantinfo", plantInfo);

            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<PlantInfo>();
            }

            return null;
        }

        public async Task<Activity> GetActivityAsync(int id)
        {
            var result = await _httpClient.GetAsync($"/api/activity/{id}");

            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<Activity>();
            }
            else
            {
                var message = result.Content.ReadAsStringAsync();
                throw new Exception(result.StatusCode + ": " + message);
            }
        }

        public async Task<Activity> PutActivityAsync(Activity activity)
        {
            var result = await _httpClient.PutAsJsonAsync("/api/activity", activity);

            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<Activity>();
            }
            else
            {
                var message = result.Content.ReadAsStringAsync();
                throw new Exception(result.StatusCode + ": " + message);
            }
        }

        public async Task<Origin> GetOriginAsync(int id)
        {
            var result = await _httpClient.GetAsync($"/api/origin/{id}");

            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<Origin>();
            }
            else
            {
                var message = result.Content.ReadAsStringAsync();
                throw new Exception(result.StatusCode + ": " + message);
            }
        }

        public async Task<Origin> PutOriginAsync(Origin origin)
        {
            var result = await _httpClient.PutAsJsonAsync("/api/origin", origin);

            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<Origin>();
            }
            else
            {
                var message = result.Content.ReadAsStringAsync();
                throw new Exception(result.StatusCode + ": " + message);
            }
        }
    }
}
