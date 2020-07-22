using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class ListSpecimensComponent : ComponentBase
    {
        [Inject]
        protected HttpClient Client { get; set; }
        public IEnumerable<Specimen> Specimens { get; set; }
        public string SearchText { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetSpecimens();
        }

        protected async Task GetSpecimens()
        {
            var url = "/api/specimen/find";
            if (!string.IsNullOrEmpty(SearchText))
            {
                url = url + "?search=" + SearchText;
            }
            var result = await Client.GetAsync(url);

            if (result.IsSuccessStatusCode)
            {
                Specimens = await result.Content.ReadFromJsonAsync<IEnumerable<Specimen>>();
            }
            else
            {
                var message = result.Content.ReadAsStringAsync();
                throw new Exception(result.StatusCode + ": " + message);
            }
        }
    }
}
