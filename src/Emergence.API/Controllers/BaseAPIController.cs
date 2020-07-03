using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Emergence.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseAPIController : ControllerBase
    {
        public string UserId => GetUserId();
        public BaseAPIController()
        {
        }

        private string GetUserId() => User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    }
}
