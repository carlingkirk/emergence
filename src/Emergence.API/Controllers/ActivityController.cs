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
        private readonly IUserService _userService;
        public ActivityController(IActivityService activityService, IPhotoService photoService, IUserService userService)
        {
            _activityService = activityService;
            _photoService = photoService;
            _userService = userService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Activity> Get(int id)
        {
            var user = await _userService.GetUserAsync(UserId);
            var activity = await _activityService.GetActivityAsync(id, user);
            var photos = await _photoService.GetPhotosAsync(PhotoType.Activity, id);

            activity.Photos = photos;

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

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetUserAsync(UserId);
            var activity = await _activityService.GetActivityAsync(id, user);
            if (activity.CreatedBy != UserId)
            {
                return Unauthorized();
            }

            await _activityService.RemoveActivityAsync(activity);

            return Ok();
        }
    }
}
