using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athame.Core.Plugin;
using Athame.PluginAPI.Service;

namespace Athame.Core.Search
{
    /// <summary>
    /// The state of the URL parser after attempting to parse a URL.
    /// </summary>
    public enum UrlParseState
    {
        /// <summary>
        /// The input was null or whitespace.
        /// </summary>
        NullOrEmptyString,
        /// <summary>
        /// The input was not a valid URL.
        /// </summary>
        InvalidUrl,
        /// <summary>
        /// The URL was valid, but no service corresponds to it.
        /// </summary>
        NoServiceFound,
        /// <summary>
        /// A service was found that matches the URL's host, but it is not authenticated.
        /// At this state and the ones that follow it, the <see cref="UrlResolver.Service"/> property
        /// is the matching service.
        /// </summary>
        ServiceNotAuthenticated,
        /// <summary>
        /// The service does not recognise this URL as pointing to any downloadable media.
        /// </summary>
        NoMedia,
        /// <summary>
        /// The URL points to a downloadable media collection.
        /// </summary>
        Success
    }

    /// <summary>
    /// Parses and resolves media URLs.
    /// </summary>
    public class UrlResolver
    {
        private readonly PluginManager pluginManager;

        /// <summary>
        /// The service the URL's host points to.
        /// </summary>
        public MusicService Service { get; private set; }

        /// <summary>
        /// The media type and ID parsed from the URL.
        /// </summary>
        public UrlParseResult ParseResult { get; private set; }

        /// <summary>
        /// If a URL has been parsed yet.
        /// </summary>
        public bool HasParsedUrl => Service != null && ParseResult != null;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="pluginManager">The <see cref="PluginManager"/> to find services from.</param>
        public UrlResolver(PluginManager pluginManager)
        {
            this.pluginManager = pluginManager;
        }

        /// <summary>
        /// Attempts to parse a URL that refers to a media collection.
        /// </summary>
        /// <param name="url">The URL to parse.</param>
        /// <returns>The state of the parser. See the documentation for <see cref="UrlParseState"/> for more info.</returns>
        public UrlParseState Parse(string url)
        {
            Service = null;
            ParseResult = null;

            if (String.IsNullOrWhiteSpace(url))
            {
                return UrlParseState.NullOrEmptyString;
            }

            Uri actualUrl;
            if (!Uri.TryCreate(url, UriKind.Absolute, out actualUrl))
            {
                return UrlParseState.InvalidUrl;
            }

            Service = pluginManager.GetServiceByBaseUri(actualUrl);
            if (Service == null)
            {
                return UrlParseState.NoServiceFound;
            }

            if (!Service.AsAuthenticatable().IsAuthenticated)
            {
                return UrlParseState.ServiceNotAuthenticated;
            }

            if ((ParseResult = Service.ParseUrl(actualUrl)) == null || ParseResult?.Type == MediaType.Unknown)
            {
                return UrlParseState.NoMedia;
            }

            return UrlParseState.Success;
        }

        /// <summary>
        /// Resolves the last parsed URL to a media collection object.
        /// </summary>
        /// <returns>An <see cref="IMediaCollection"/> according to the <see cref="UrlParseResult.Type"/> property.</returns>
        public async Task<IMediaCollection> Resolve()
        {
            if (!HasParsedUrl)
            {
                throw new InvalidOperationException("Parse(string) must be called first.");
            }
            switch (ParseResult.Type)
            {
                case MediaType.Unknown:
                    throw new InvalidOperationException("Can't resolve unknown media type");
                    
                case MediaType.Album:
                    return await Service.GetAlbumAsync(ParseResult.Id, true);
                    
                case MediaType.Track:
                    return (await Service.GetTrackAsync(ParseResult.Id)).AsCollection();

                case MediaType.Playlist:
                    var playlist = await Service.GetPlaylistAsync(ParseResult.Id);
                    if (playlist.Tracks == null)
                    {
                        var items = Service.GetPlaylistItems(ParseResult.Id, 100);
                        await items.LoadAllPagesAsync();
                        playlist.Tracks = items.AllItems;
                    }
                    return playlist;

                case MediaType.Artist:
                    throw new NotImplementedException("Artist URLs aren't currently implemented.");

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
