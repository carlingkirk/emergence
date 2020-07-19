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
        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Activity> Get(int id)
        {
            var activity = await _activityService.GetActivityAsync(id);
            return activity;
        }


        [HttpPut]

        public async Task<Activity> Put(Activity activity) => await _activityService.AddOrUpdateActivityAsync(activity, UserId);
    }
}
