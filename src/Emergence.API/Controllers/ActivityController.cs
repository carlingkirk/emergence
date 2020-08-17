using System.Linq;
using System.Threading.Tasks;
using Emergence.Data.Shared;
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

        [HttpPost]
        [Route("Find")]
        public async Task<FindResult<Activity>> FindActivities(FindParams findParams, int? specimenId)
        {
            var result = await _activityService.FindActivities(findParams, UserId, specimenId);
            return result;
        }
    }
}
