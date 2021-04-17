using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Emergence.Models
{
    /// <summary>
    /// The Web App Manifest
    /// </summary>
    public class WebManifest
    {
        /// <summary>The absolute file path to Web App Manifest file.</summary>
        [JsonIgnore]
        public string FileName { get; internal set; }

        /// <summary>The raw JSON from the manifest file.</summary>
        [JsonIgnore]
        public string RawJson { get; internal set; }

        /// <summary>A name for use in the Web App Install banner.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>A short_name for use as the text on the users home screen.</summary>
        [JsonProperty("short_name")]
        public string ShortName { get; set; }

        /// <summary>Provides a general description of what the web application does.</summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>.</summary>
        [JsonProperty("iarc_rating_id")]
        public string IarcRatingId { get; set; }

        /// <summary>.</summary>
        [JsonProperty("categories")]
        public IEnumerable<string> Categories { get; set; }

        /// <summary>Specifies the primary text direction for the name, short_name, and description members.
        /// Together with the lang member, it can help provide the correct display of right-to-left languages.</summary>
        [JsonProperty("dir")]
        public string Dir { get; set; }

        /// <summary>Specifies the primary language for the values in the name and short_name members. This value is a string containing a single language tag.</summary>
        [JsonProperty("lang")]
        public string Lang { get; set; }

        /// <summary>If you don't provide a start_url, the current page is used, which is unlikely to be what your users want.</summary>
        [JsonProperty("start_url")]
        public string StartUrl { get; set; }

        /// <summary>A list of icons.</summary>
        [JsonProperty("icons")]
        public IEnumerable<Icon> Icons { get; set; }

        /// <summary>A hex color value.</summary>
        [JsonProperty("background_color")]
        public string BackgroundColor { get; set; }

        /// <summary>A hex color value.</summary>
        [JsonProperty("theme_color")]
        public string ThemeColor { get; set; }

        /// <summary>Defines the developer's preferred display mode for the web application.</summary>
        [JsonProperty("display")]
        public string Display { get; set; }

        /// <summary></summary>
        [JsonProperty("orientation")]
        public string Orientation { get; set; }

        /// <summary>pecifies a boolean value that hints for the user agent to indicate to the user that the specified related applications are available, and recommended over the web application.</summary>
        [JsonProperty("prefer_related_applications")]
        public bool PreferRelatedApplications { get; set; }

        /// <summary>Specifies an array of "application objects" representing native applications that are installable by, or accessible to, the underlying platform.</summary>
        [JsonProperty("related_applications")]
        public IEnumerable<RelatedApplication> RelatedApplications { get; set; }

        /// <summary>Defines the navigation scope of this web application's application context.</summary>
        [JsonProperty("scope")]
        public string Scope { get; set; }

        /// <summary>
        /// Check if the manifest is valid
        /// </summary>
        public bool IsValid(out string error)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(ShortName) || string.IsNullOrEmpty(StartUrl) || Icons == null)
            {
                error = $"The fields 'name', 'short_name', 'start_url' and 'icons' must be set  in {FileName}";
                return false;
            }

            if (!Icons.Any(i => i.Sizes?.Equals("512x512", StringComparison.OrdinalIgnoreCase) == true))
            {
                error = $"Missing icon in size 512x512 in {FileName}";
                return false;
            }

            if (!Icons.Any(i => i.Sizes?.Equals("192x192", StringComparison.OrdinalIgnoreCase) == true))
            {
                error = $"Missing icon in size 192x192 in {FileName}";
                return false;
            }

            error = "";
            return true;
        }
    }

    /// <summary>
    /// An icon as defined in the web manifest
    /// </summary>
    public class Icon
    {
        /// <summary>The path to the image file. If src is a relative URL, the base URL will be the URL of the manifest. Example: "/img/icon-192x192.png"</summary>
        [JsonProperty("src")]
        public string Src { get; set; }

        /// <summary>A hint as to the media type of the image.The purpose of this member is to allow a user agent to quickly ignore images of media types it does not support. Example: "image/png"</summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>A string containing space-separated image dimensions. Example: "192x192"</summary>
        [JsonProperty("sizes")]
        public string Sizes { get; set; }
    }

    /// <summary>
    /// A related native application.
    /// </summary>
    public class RelatedApplication
    {
        /// <summary>The platform on which the application can be found.</summary>
        [JsonProperty("platform")]
        public string Platform { get; set; }

        /// <summary>The URL at which the application can be found.</summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>The ID used to represent the application on the specified platform.</summary>
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    internal class WebManifestCache
    {
        private readonly IWebHostEnvironment _env;
        private readonly MemoryCache _cache;
        private readonly string _fileName;

        public WebManifestCache(IWebHostEnvironment env, string fileName)
        {
            _env = env;
            _fileName = fileName;
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        public WebManifest GetManifest() =>
            _cache.GetOrCreate("webmanifest", (entry) =>
            {
                var file = _env.WebRootFileProvider.GetFileInfo(_fileName);
                entry.AddExpirationToken(_env.WebRootFileProvider.Watch(_fileName));

                var json = File.ReadAllText(file.PhysicalPath);

                var manifest = JsonConvert.DeserializeObject<WebManifest>(json);
                manifest.FileName = _fileName;
                manifest.RawJson = Regex.Replace(json, "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");

                if (!manifest.IsValid(out var error))
                {
                    throw new JsonException(error);
                }

                return manifest;
            });
    }
}
