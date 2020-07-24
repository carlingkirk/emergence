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

        public async Task<IEnumerable<Origin>> FindOrigins(string searchText)
        {
            var origins = (await _httpClient.GetFromJsonAsync<IEnumerable<Origin>>($"/api/origin/find?search={searchText}&skip=0&take=10")).ToList();

            origins.Add(new Origin { Name = searchText });
            return origins;
        }

        public async Task<IEnumerable<Lifeform>> FindLifeforms(string searchText)
        {
            var lifeforms = (await _httpClient.GetFromJsonAsync<IEnumerable<Lifeform>>($"/api/lifeform/find?search={searchText}&skip=0&take=10")).ToList();

            return lifeforms;
        }

        public async Task<Specimen> GetSpecimen(int id)
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

        public async Task<Specimen> PutSpecimen(Specimen specimen)
        {
            var result = await _httpClient.PutAsJsonAsync("/api/specimen", specimen);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<Specimen>();
            }

            return null;
        }

        public async Task<PlantInfo> GetPlantInfo(int id)
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

        public async Task<PlantInfo> PutPlantInfo(PlantInfo plantInfo)
        {
            var result = await _httpClient.PutAsJsonAsync("/api/plantinfo", plantInfo);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<PlantInfo>();
            }

            return null;
        }
    }
}
