using System;
using Microsoft.Extensions.Configuration;

namespace Emergence.Models
{
    /// <summary>
    /// Options for the service worker.
    /// </summary>
    public class PwaOptions
    {
        /// <summary>
        /// Creates a new default instance of the options.
        /// </summary>
        public PwaOptions()
        {
            CacheId = Constants.DefaultCacheId;
            Strategy = ServiceWorkerStrategy.CacheFirstSafe;
            RoutesToPreCache = "/";
            BaseRoute = "";
            OfflineRoute = Constants.Offlineroute;
            RegisterServiceWorker = true;
            RegisterWebmanifest = true;
            EnableCspNonce = false;
            ServiceWorkerCacheControlMaxAge = 60 * 60 * 24 * 30;    // 30 days
            WebManifestCacheControlMaxAge = 60 * 60 * 24 * 30;      // 30 days
            CustomServiceWorkerStrategyFileName = Constants.CustomServiceworkerFileName;
            RoutesToIgnore = "";
        }

        internal PwaOptions(IConfiguration config)
            : this()
        {
            CacheId = config["pwa:cacheId"] ?? CacheId;
            RoutesToPreCache = config["pwa:routesToPreCache"] ?? RoutesToPreCache;
            BaseRoute = config["pwa:baseRoute"] ?? BaseRoute;
            OfflineRoute = config["pwa:offlineRoute"] ?? OfflineRoute;
            RoutesToIgnore = config["pwa:routesToIgnore"] ?? RoutesToIgnore;
            CustomServiceWorkerStrategyFileName =
                config["pwa:customServiceWorkerFileName"] ?? CustomServiceWorkerStrategyFileName;

            if (bool.TryParse(config["pwa:registerServiceWorker"] ?? "true", out var register))
            {
                RegisterServiceWorker = register;
            }

            if (bool.TryParse(config["pwa:registerWebmanifest"] ?? "true", out var manifest))
            {
                RegisterWebmanifest = manifest;
            }

            if (bool.TryParse(config["pwa:EnableCspNonce"] ?? "true", out var enableCspNonce))
            {
                EnableCspNonce = enableCspNonce;
            }

            if (Enum.TryParse(config["pwa:strategy"] ?? "cacheFirstSafe", true, out ServiceWorkerStrategy mode))
            {
                Strategy = mode;
            }

            if (int.TryParse(config["pwa:ServiceWorkerCacheControlMaxAge"], out var serviceWorkerCacheControlMaxAge))
            {
                ServiceWorkerCacheControlMaxAge = serviceWorkerCacheControlMaxAge;
            }

            if (int.TryParse(config["pwa:WebManifestCacheControlMaxAge"], out var webManifestCacheControlMaxAge))
            {
                WebManifestCacheControlMaxAge = webManifestCacheControlMaxAge;
            }
        }

        /// <summary>
        /// The cache identifier of the service worker (can be any string).
        /// Change this property to force the service worker to reload in browsers.
        /// </summary>
        public string CacheId { get; set; }

        /// <summary>
        /// Selects one of the predefined service worker types.
        /// </summary>
        public ServiceWorkerStrategy Strategy { get; set; }

        /// <summary>
        /// A comma separated list of routes to pre-cache when service worker installs in the browser.
        /// </summary>
        public string RoutesToPreCache { get; set; }

        /// <summary>
        /// The base route to the application.
        /// </summary>
        public string BaseRoute { get; set; }

        /// <summary>
        /// The route to the page to show when offline.
        /// </summary>
        public string OfflineRoute { get; set; }

        /// <summary>
        /// Determines if a script that registers the service worker should be injected
        /// into the bottom of the HTML page.
        /// </summary>
        public bool RegisterServiceWorker { get; set; }

        /// <summary>
        /// Determines if a meta tag that points to the web manifest should be inserted
        /// at the end of the head element.
        /// </summary>
        public bool RegisterWebmanifest { get; set; }

        /// <summary>
        /// Determines the value of the ServiceWorker CacheControl header Max-Age (in seconds)
        /// </summary>
        public int ServiceWorkerCacheControlMaxAge { get; set; }

        public int WebManifestCacheControlMaxAge { get; set; }

        /// <summary>
        /// Determines whether a CSP nonce will be added via NWebSec
        /// </summary>
        public bool EnableCspNonce { get; set; }

        /// <summary>
        /// Generate code even on HTTP connection. Necessary for SSL offloading.
        /// </summary>
        public bool AllowHttp { get; set; }

        /// <summary>
        /// The file name of the Custom ServiceWorker Strategy
        /// </summary>
        public string CustomServiceWorkerStrategyFileName { get; set; }

        /// <summary>
        /// A comma separated list of routes to ignore when implementing a CustomServiceworker.  
        /// </summary>
        public string RoutesToIgnore { get; set; }
    }

    /// <summary>
    /// The various modes of service workers.
    /// </summary>
    public enum ServiceWorkerStrategy
    {
        /// <summary>
        /// Serves all resources from cache and falls back to network.
        /// </summary>
        CacheFirst,

        /// <summary>
        /// Caches all resources and serves from the cache resources with ?v=... query string. Checks network first for HTML.
        /// </summary>
        CacheFirstSafe,

        /// <summary>
        /// Caches resources with ?v=... query string only. Unlike <see cref="CacheFirstSafe"/>, this doesn't cache resources without fingerprints.
        /// </summary>
        CacheFingerprinted,

        /// <summary>
        /// The minimal strategy does nothing and is good for when you only want a service worker in
        /// order for browsers to suggest installing your PWA.
        /// </summary>
        Minimal,

        /// <summary>
        /// Always tries the network first and falls back to cache when offline.
        /// </summary>
        NetworkFirst,

        /// <summary>
        /// Allows a user defined custom strategy to be provided.
        /// </summary>
        CustomStrategy
    }

    public static class Constants
    {
        public const string ServiceworkerRoute = "/serviceworker";
        public const string CustomServiceworkerFileName = "customserviceworker.js";
        public const string Offlineroute = "/offline.html";
        public const string DefaultCacheId = "v1.0";
        public const string WebManifestRoute = "/manifest.webmanifest";
        public const string WebManifestFileName = "manifest.json";
        public const string CspNonce = " nws-csp-add-nonce='true'";
    }
}
