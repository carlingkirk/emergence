using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class ListActivitiesComponent : ComponentBase
    {
        [Inject]
        protected HttpClient Client { get; set; }
        public IEnumerable<Activity> Activities { get; set; }
        public string SearchText { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetActivities();
        }

        protected async Task GetActivities()
        {
            var url = "/api/activity/find";
            if (!string.IsNullOrEmpty(SearchText))
            {
                url = url + "?search=" + SearchText;
            }
            var result = await Client.GetAsync(url);

            if (result.IsSuccessStatusCode)
            {
                Activities = await result.Content.ReadFromJsonAsync<IEnumerable<Activity>>();
            }
            else
            {
                var message = result.Content.ReadAsStringAsync();
                throw new Exception(result.StatusCode + ": " + message);
            }
        }
    }
}
