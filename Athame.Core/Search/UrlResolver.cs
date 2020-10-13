using System;
using System.Threading.Tasks;
using Athame.Core.Plugin;
using Athame.Plugin.Api.Service;

namespace Athame.Core.Search
{
    /// <summary>
    /// Parses and resolves media URLs.
    /// </summary>
    public class UrlResolver
    {
        private readonly MediaServiceManager serviceManager;
        private readonly AuthenticationManager authManager;

        /// <summary>
        /// The service the URL's host points to.
        /// </summary>
        public IMediaService Service { get; private set; }

        /// <summary>
        /// The media type and media ID parsed from the URL.
        /// </summary>
        public MediaUri MediaUri { get; private set; }

        /// <summary>
        /// The exception thrown by the service.
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// If a URL has been parsed yet.
        /// </summary>
        public bool HasParsedUrl 
            => Service != null && MediaUri != null;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="serviceManager">The <see cref="MediaServiceManager"/> to find services from.</param>
        /// <param name="authManager">The <see cref="AuthenticationManager"/> to use.</param>
        public UrlResolver(MediaServiceManager serviceManager, AuthenticationManager authManager)
        {
            this.serviceManager = serviceManager;
            this.authManager = authManager;
        }

        /// <summary>
        /// Attempts to parse a URL that refers to a media collection.
        /// </summary>
        /// <param name="url">The URL to parse.</param>
        /// <returns>The state of the parser. See the documentation for <see cref="UrlParseResult"/> for more info.</returns>
        public UrlParseResult Parse(string url)
        {
            Service = null;
            MediaUri = null;
            Exception = null;

            if (string.IsNullOrWhiteSpace(url))
            {
                return UrlParseResult.NullOrEmptyString;
            }

            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri actualUrl))
            {
                return UrlParseResult.InvalidUrl;
            }

            Service = serviceManager.GetService(actualUrl);
            if (Service == null)
            {
                return UrlParseResult.NoServiceFound;
            }

            // if (_authManager.CanRestore(Service))
            //     return UrlParseResult.ServiceNotRestored;

            // if (authManager.NeedsAuthentication(Service))
            // {
            //     return UrlParseResult.ServiceNotAuthenticated;
            // }

            try
            {
                MediaUri = Service.ParseUrl(actualUrl);
                if (MediaUri == null || MediaUri.MediaType == MediaType.Unknown)
                {
                    return UrlParseResult.NoMedia;
                }
            }
            catch (Exception ex)
            {
                Exception = ex;
                return UrlParseResult.Exception;
            }

            return UrlParseResult.Success;
        }

        /// <summary>
        /// Resolves the last parsed URL to a media collection object.
        /// </summary>
        /// <returns>An <see cref="IMediaCollection"/> according to the <see cref="MediaUri.MediaType"/> property.</returns>
        public async Task<IMediaCollection> ResolveAsync() =>
            MediaUri.MediaType switch
            {
                MediaType.Album
                    => await Service.GetAlbumAsync(MediaUri.MediaId, withTracks: true).ConfigureAwait(false),
                MediaType.Track
                    => (await Service.GetTrackAsync(MediaUri.MediaId).ConfigureAwait(false)).AsCollection(),
                MediaType.Playlist
                    => await Service.GetPlaylistAsync(MediaUri.MediaId).ConfigureAwait(false),
                MediaType.Artist
                    => throw new NotImplementedException("Artist URLs aren't currently implemented."),
                _   => throw new InvalidOperationException("Can't resolve unknown media type")
            };
    }
}
