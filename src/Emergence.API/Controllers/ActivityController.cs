using System.Threading.Tasks;
using Emergence.API.Services.Interfaces;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Emergence.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;
        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        [HttpGet]
        public async Task<Activity> Get(int id) => await _activityService.GetActivityAsync(id);

        [HttpPut]

        public async Task<Activity> Put(Activity activity) => await _activityService.AddOrUpdateActivityAsync(activity);
    }
}
