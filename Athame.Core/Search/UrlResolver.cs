using System;
using Athame.Core.Interface;
using Athame.Core.Plugin;
using Athame.Plugin.Api.Service;

namespace Athame.Core.Search
{
    /// <summary>
    /// Parses and resolves media URLs.
    /// </summary>
    public static class UrlResolver
    {
        /// <summary>
        /// Attempts to parse a URL that refers to a media collection.
        /// </summary>
        /// <param name="url">The URL to parse.</param>
        /// <returns>The state of the parser. See the documentation for <see cref="IUrlParseResult"/> for more info.</returns>
        public static IUrlParseResult ResolveUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return UrlParse.Empty;
            }

            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri actualUrl))
            {
                return UrlParse.Invalid;
            }

            var service = AthameApp.GetService(actualUrl);
            if (service == null)
            {
                return UrlParse.NoService;
            }

            var descriptor = service.ParseUrl(actualUrl);
            if (descriptor == null || descriptor.MediaType == MediaType.Unknown)
            {
                return UrlParse.NoMedia;
            }

            return UrlParse.Success(descriptor);
        }
    }
}
