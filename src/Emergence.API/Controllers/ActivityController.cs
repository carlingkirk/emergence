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
            var user = await _userService.GetIdentifyingUser(UserId);
            var activity = await _activityService.GetActivityAsync(id, user);

            if (activity.User?.PhotoId != null)
            {
                var userPhoto = await _photoService.GetPhotoAsync(activity.User.PhotoId.Value);
                activity.User.PhotoThumbnailUri = userPhoto.ThumbnailUri;
            }

            var photos = await _photoService.GetPhotosAsync(PhotoType.Activity, id);

            activity.Photos = photos;

            return activity;
        }

        [HttpPut]
        public async Task<Activity> Put(Activity activity)
        {
            activity.CreatedBy = UserId;
            activity.UserId = await _userService.GetUserIdAsync(UserId);
            var activityResult = await _activityService.AddOrUpdateActivityAsync(activity);

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
            var user = await _userService.GetIdentifyingUser(UserId);
            var result = await _activityService.FindActivities(findParams, user, specimenId);
            return result;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetIdentifyingUser(UserId);
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
