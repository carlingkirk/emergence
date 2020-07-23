using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class ListOriginsComponent : ComponentBase
    {
        [Inject]
        protected HttpClient Client { get; set; }
        public IEnumerable<Origin> Origins { get; set; }
        public string SearchText { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetOrigins();
        }

        protected async Task GetOrigins()
        {
            var url = "/api/origin/find";
            if (!string.IsNullOrEmpty(SearchText))
            {
                url = url + "?search=" + SearchText;
            }
            var result = await Client.GetAsync(url);

            if (result.IsSuccessStatusCode)
            {
                Origins = await result.Content.ReadFromJsonAsync<IEnumerable<Origin>>();
            }
            else
            {
                var message = result.Content.ReadAsStringAsync();
                throw new Exception(result.StatusCode + ": " + message);
            }
        }
    }
}
