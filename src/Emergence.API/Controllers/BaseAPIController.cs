using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Emergence.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        public string UserId => GetUserId();
        public BaseApiController()
        {
        }

        private string GetUserId() => User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    }
}
