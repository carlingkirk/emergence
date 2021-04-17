using System.IO;
using System.Threading.Tasks;
using Emergence.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Emergence.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PwaController : ControllerBase
    {
        private readonly PwaOptions _options;

        /// <summary>
        /// Creates an instance of the controller.
        /// </summary>
        public PwaController(PwaOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// Serves a service worker based on the provided settings.
        /// </summary>
        [Route(Constants.ServiceworkerRoute)]
        [HttpGet]
        public async Task<IActionResult> ServiceWorkerAsync()
        {
            Response.ContentType = "application/javascript; charset=utf-8";
            Response.Headers[HeaderNames.CacheControl] = $"max-age={_options.ServiceWorkerCacheControlMaxAge}";


            var fileName = _options.Strategy + ".js";
            var assembly = typeof(PwaController).Assembly;
            var resourceStream = assembly.GetManifestResourceStream($"Emergence.Resources.{fileName}");

            using (var reader = new StreamReader(resourceStream))
            {
                var fileContent = await reader.ReadToEndAsync();
                var serviceWorkerResource = fileContent.Replace("{version}", _options.CacheId + "::" + _options.Strategy);
                return Content(serviceWorkerResource);
            }
        }

        /// <summary>
        /// Serves the offline.html file
        /// </summary>
        [Route(Constants.Offlineroute)]
        [HttpGet]
        public async Task<IActionResult> OfflineAsync()
        {
            Response.ContentType = "text/html";

            var assembly = typeof(PwaController).Assembly;
            var resourceStream = assembly.GetManifestResourceStream("Emergence.Resources.offline.html");

            using (var reader = new StreamReader(resourceStream))
            {
                return Content(await reader.ReadToEndAsync());
            }
        }

        /// <summary>
        /// Serves the manifest.json file
        /// </summary>
        [Route(Constants.WebManifestRoute)]
        [HttpGet]
        public IActionResult WebManifest([FromServices] WebManifest wm)
        {
            if (wm == null)
            {
                return NotFound();
            }

            Response.ContentType = "application/manifest+json; charset=utf-8";

            Response.Headers[HeaderNames.CacheControl] = $"max-age={_options.WebManifestCacheControlMaxAge}";

            return Content(wm.RawJson);
        }
    }
}
