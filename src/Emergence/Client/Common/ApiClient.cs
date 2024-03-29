using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorInputFile;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Enums;
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

        public async Task<FindResult<Activity>> FindActivitiesAsync(FindParams findParams)
        {
            var result = await _httpClient.PostAsJsonAsync($"/api/activity/find", findParams);

            return await ReadResult<FindResult<Activity>>(result);
        }

        public async Task<FindResult<Activity>> FindActivitiesAsync(FindParams findParams, Specimen specimen)
        {
            var result = await _httpClient.PostAsJsonAsync($"/api/activity/find?specimenId={specimen.SpecimenId}", findParams);

            return await ReadResult<FindResult<Activity>>(result);
        }

        public async Task<FindResult<Activity>> FindScheduledActivitiesAsync(FindParams findParams, DateTime date)
        {
            var result = await _httpClient.PostAsJsonAsync($"/api/activity/find/scheduled?date={date.Ticks}", findParams);

            return await ReadResult<FindResult<Activity>>(result);
        }

        public async Task<FindResult<Lifeform>> FindLifeformsAsync(FindParams<Lifeform> findParams)
        {
            var result = await _httpClient.PostAsJsonAsync($"/api/lifeform/find", findParams);

            return await ReadResult<FindResult<Lifeform>>(result);
        }

        public async Task<FindResult<Origin>> FindOriginsAsync(FindParams findParams)
        {
            var result = await _httpClient.PostAsJsonAsync($"/api/origin/find", findParams);

            return await ReadResult<FindResult<Origin>>(result);
        }

        public async Task<PlantInfoFindResult> FindPlantInfosAsync(PlantInfoFindParams findParams)
        {
            var result = await _httpClient.PostAsJsonAsync($"/api/plantinfo/find", findParams);

            return await ReadResult<PlantInfoFindResult>(result);
        }

        public async Task<SpecimenFindResult> FindSpecimensAsync(SpecimenFindParams findParams)
        {
            var result = await _httpClient.PostAsJsonAsync($"/api/specimen/find", findParams);

            return await ReadResult<SpecimenFindResult>(result);
        }

        public async Task<FindResult<Taxon>> FindTaxonsAsync(FindParams<Taxon> findParams, TaxonRank rank)
        {
            var result = await _httpClient.PostAsJsonAsync($"/api/taxon/find?rank={rank}", findParams);

            return await ReadResult<FindResult<Taxon>>(result);
        }

        public async Task<FindResult<UserContact>> FindContactsAsync(FindParams findParams)
        {
            var result = await _httpClient.PostAsJsonAsync($"/api/usercontact/find", findParams);

            return await ReadResult<FindResult<UserContact>>(result);
        }

        public async Task<FindResult<UserContactRequest>> FindContactRequestsAsync(FindParams findParams)
        {
            var result = await _httpClient.PostAsJsonAsync($"/api/usercontact/request/find", findParams);

            return await ReadResult<FindResult<UserContactRequest>>(result);
        }

        public async Task<FindResult<UserMessage>> FindMessagesAsync(FindParams findParams)
        {
            var result = await _httpClient.PostAsJsonAsync($"/api/message/find", findParams);

            return await ReadResult<FindResult<UserMessage>>(result);
        }

        public async Task<FindResult<UserMessage>> FindSentMessagesAsync(FindParams findParams)
        {
            var result = await _httpClient.PostAsJsonAsync($"/api/message/sent/find", findParams);

            return await ReadResult<FindResult<UserMessage>>(result);
        }

        public async Task<FindResult<UserSummary>> FindUsersAsync(FindParams findParams)
        {
            var result = await _httpClient.PostAsJsonAsync($"/api/user/find", findParams);

            return await ReadResult<FindResult<UserSummary>>(result);
        }

        public async Task<Specimen> GetSpecimenAsync(int id)
        {
            var result = await _httpClient.GetAsync($"/api/specimen/{id}");

            return await ReadResult<Specimen>(result);
        }

        public async Task<Specimen> PutSpecimenAsync(Specimen specimen)
        {
            var result = await _httpClient.PutAsJsonAsync("/api/specimen", specimen);

            return await ReadResult<Specimen>(result);
        }

        public async Task<bool> RemoveSpecimenAsync(Specimen specimen)
        {
            var result = await _httpClient.DeleteAsync($"/api/specimen/{specimen.SpecimenId}");

            _ = await ReadResult(result);

            return true;
        }

        public async Task<Lifeform> GetLifeformAsync(int id)
        {
            var result = await _httpClient.GetAsync($"/api/lifeform/{id}");

            return await ReadResult<Lifeform>(result);
        }

        public async Task<PlantInfo> GetPlantInfoAsync(int id)
        {
            var result = await _httpClient.GetAsync($"/api/plantinfo/{id}");

            return await ReadResult<PlantInfo>(result);
        }

        public async Task<PlantInfo> PutPlantInfoAsync(PlantInfo plantInfo)
        {
            var result = await _httpClient.PutAsJsonAsync("/api/plantinfo", plantInfo);

            return await ReadResult<PlantInfo>(result);
        }

        public async Task<Activity> GetActivityAsync(int id)
        {
            var result = await _httpClient.GetAsync($"/api/activity/{id}");

            return await ReadResult<Activity>(result);
        }

        public async Task<Activity> PutActivityAsync(Activity activity)
        {
            var result = await _httpClient.PutAsJsonAsync("/api/activity", activity);

            return await ReadResult<Activity>(result);
        }

        public async Task<Origin> GetOriginAsync(int id)
        {
            var result = await _httpClient.GetAsync($"/api/origin/{id}");

            return await ReadResult<Origin>(result);
        }

        public async Task<Origin> PutOriginAsync(Origin origin)
        {
            var result = await _httpClient.PutAsJsonAsync("/api/origin", origin);

            return await ReadResult<Origin>(result);
        }

        public async Task<bool> RemoveOriginAsync(Origin origin)
        {
            var result = await _httpClient.DeleteAsync($"/api/origin/{origin.OriginId}");

            _ = await ReadResult(result);

            return true;
        }

        public async Task<IEnumerable<Photo>> UploadPhotosAsync(IFileListEntry[] photos, PhotoType type)
        {
            using (var content = new MultipartFormDataContent())
            {
                foreach (var photo in photos)
                {
                    var photoContent = new StreamContent(photo.Data);
                    photoContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "photos",
                        FileName = photo.Name
                    };
                    photoContent.Headers.ContentType = new MediaTypeHeaderValue(photo.Type);

                    content.Add(photoContent);
                }

                var result = await _httpClient.PostAsync($"/api/photo/{type}/uploadmany", content);

                return await ReadResult<IEnumerable<Photo>>(result);
            }
        }

        public async Task<Photo> UploadPhotoAsync(IFileListEntry photo, PhotoType type)
        {
            using (var content = new MultipartFormDataContent())
            using (var photoContent = new StreamContent(photo.Data))
            {
                photoContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "photo",
                    FileName = photo.Name
                };
                photoContent.Headers.ContentType = new MediaTypeHeaderValue(photo.Type);
                content.Add(photoContent);

                var result = await _httpClient.PostAsync($"/api/photo/{type}/upload", content);

                return await ReadResult<Photo>(result);
            }
        }

        public async Task<Photo> AddExternalPhotoAsync(string url, PhotoType type)
        {
            var result = await _httpClient.PostAsJsonAsync($"/api/photo/{type}/addexternal", new Photo { ExternalUrl = url });

            return await ReadResult<Photo>(result);
        }

        public async Task<IEnumerable<Photo>> GetPhotosAsync(PhotoType type, int id)
        {
            var result = await _httpClient.GetAsync($"/api/photo/{type}/{id}");

            return await ReadResult<IEnumerable<Photo>>(result);
        }

        public async Task<bool> RemovePhotoAsync(int id)
        {
            var result = await _httpClient.DeleteAsync($"/api/photo/{id}");

            if (result.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<User> GetUserAsync(string userId)
        {
            var result = await _httpClient.GetAsync($"/api/user/{userId}");

            return await ReadResult<User>(result);
        }

        public async Task<User> GetUserAsync(int id)
        {
            var result = await _httpClient.GetAsync($"/api/user/get?id={id}");

            return await ReadResult<User>(result);
        }

        public async Task<User> GetUserByNameAsync(string name)
        {
            var result = await _httpClient.GetAsync($"/api/user/get?name={name}");

            return await ReadResult<User>(result);
        }

        public async Task<bool> RemoveActivityAsync(Activity activity)
        {
            var result = await _httpClient.DeleteAsync($"/api/activity/{activity.ActivityId}");

            _ = await ReadResult(result);

            return true;
        }

        public async Task<bool> RemovePlantInfoAsync(PlantInfo plantInfo)
        {
            var result = await _httpClient.DeleteAsync($"/api/plantinfo/{plantInfo.PlantInfoId}");

            _ = await ReadResult(result);

            return true;
        }

        public async Task<UserContactRequest> AddContactRequestAsync(UserContactRequest userContactRequest)
        {
            var result = await _httpClient.PostAsJsonAsync($"/api/usercontact/request", userContactRequest);

            return await ReadResult<UserContactRequest>(result);
        }

        public async Task<UserContact> AddContactAsync(UserContactRequest userContactRequest)
        {
            var result = await _httpClient.PostAsJsonAsync($"/api/usercontact", userContactRequest);

            return await ReadResult<UserContact>(result);
        }

        public async Task<bool> RemoveContactAsync(UserContact userContact)
        {
            var result = await _httpClient.DeleteAsync($"/api/usercontact/{userContact.Id}");

            _ = await ReadResult(result);

            return true;
        }

        public async Task<bool> RemoveContactRequestAsync(UserContactRequest userContactRequest)
        {
            var result = await _httpClient.DeleteAsync($"/api/usercontact/request/{userContactRequest.Id}");

            _ = await ReadResult(result);

            return true;
        }

        public async Task<UserMessage> SendMessageAsync(UserMessage message)
        {
            var result = await _httpClient.PostAsJsonAsync($"/api/message", message);

            return await ReadResult<UserMessage>(result);
        }

        private async Task<T> ReadResult<T>(HttpResponseMessage result, JsonSerializerOptions options = null)
        {
            if (result.IsSuccessStatusCode)
            {
                var response = await result.Content.ReadFromJsonAsync<T>(options);
                return response;
            }
            else
            {
                var message = await result.Content.ReadAsStringAsync();
                throw new HttpRequestException(result.StatusCode + ": " + message);
            }
        }

        private async Task<string> ReadResult(HttpResponseMessage result)
        {
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadAsStringAsync();
            }
            else
            {
                var message = await result.Content.ReadAsStringAsync();
                throw new HttpRequestException(result.StatusCode + ": " + message);
            }
        }
    }
}
