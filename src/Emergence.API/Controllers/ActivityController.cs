using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;
using Emergence.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Emergence.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : BaseAPIController
    {
        private readonly IActivityService _activityService;
        private readonly IPhotoService _photoService;
        public ActivityController(IActivityService activityService, IPhotoService photoService)
        {
            _activityService = activityService;
            _photoService = photoService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Activity> Get(int id)
        {
            var activity = await _activityService.GetActivityAsync(id);
            return activity;
        }

        [HttpPut]

        public async Task<Activity> Put(Activity activity)
        {
            var activityResult = await _activityService.AddOrUpdateActivityAsync(activity, UserId);

            if (activity.Photos != null && activity.Photos.Any())
            {
                foreach (var photo in activity.Photos)
                {
                    photo.TypeId = activityResult.ActivityId;
                }

                activityResult.Photos = await _photoService.AddOrUpdatePhotosAsync(activity.Photos);
            }

            return activityResult;
        }

        [HttpGet]
        [Route("Find")]
        public async Task<IEnumerable<Activity>> FindActivities(string search = null, int skip = 0, int take = 10, string sortBy = null,
            SortDirection sortDir = SortDirection.Ascending)
        {
            var results = await _activityService.FindActivities(search, UserId, skip, take, sortBy, sortDir);
            return results;
        }
    }
}
