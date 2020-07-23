using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class ListPlantInfosComponent : ComponentBase
    {
        [Inject]
        protected HttpClient Client { get; set; }
        public IEnumerable<PlantInfo> PlantInfos { get; set; }
        public string SearchText { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetPlantInfos();
        }

        protected async Task GetPlantInfos()
        {
            var url = "/api/plantInfo/find";
            if (!string.IsNullOrEmpty(SearchText))
            {
                url += "?search=" + SearchText + "&take=50";
            }
            else
            {
                url += "?take=50";
            }

            var result = await Client.GetAsync(url);

            if (result.IsSuccessStatusCode)
            {
                PlantInfos = await result.Content.ReadFromJsonAsync<IEnumerable<PlantInfo>>();
            }
            else
            {
                var message = result.Content.ReadAsStringAsync();
                throw new Exception(result.StatusCode + ": " + message);
            }
        }
    }
}
