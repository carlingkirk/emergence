using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Emergence.Models
{
    internal class WebmanifestTagHelperComponent : TagHelperComponent
    {
        private readonly string _link;
        private const string ThemeFormat = "\t<meta name=\"theme-color\" content=\"{0}\" />\r\n";
        private readonly PwaOptions _options;
        private readonly IServiceProvider _serviceProvider;

        public WebmanifestTagHelperComponent(PwaOptions options, IServiceProvider serviceProvider)
        {
            _options = options;
            _link = "\t<link rel=\"manifest\" href=\"" + options.BaseRoute + Constants.WebManifestRoute + "\" />\r\n";
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public override int Order => 100;

        /// <inheritdoc />
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!_options.RegisterWebmanifest)
            {
                return;
            }

            if (!(_serviceProvider.GetService(typeof(WebManifest)) is WebManifest manifest))
            {
                return;
            }

            if (string.Equals(context.TagName, "head", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrEmpty(manifest.ThemeColor))
                {
                    output.PostContent.AppendHtml(string.Format(ThemeFormat, manifest.ThemeColor));
                }

                output.PostContent.AppendHtml(_link);
            }
        }
    }
}
