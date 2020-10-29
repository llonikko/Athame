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
        /// <summary>
        /// Attempts to parse a URL that refers to a media collection.
        /// </summary>
        /// <param name="url">The URL to parse.</param>
        /// <returns>The state of the parser. See the documentation for <see cref="UrlParseResult"/> for more info.</returns>
        public UrlParseResult Parse(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return new UrlParseResult(UrlParseStatus.NullOrEmptyString);
            }

            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri actualUrl))
            {
                return new UrlParseResult(UrlParseStatus.InvalidUrl);
            }

            var service = MediaServiceManager.GetService(actualUrl);
            if (service == null)
            {
                return new UrlParseResult(UrlParseStatus.NoServiceFound);
            }

            var descriptor = service.ParseUrl(actualUrl);
            if (descriptor == null || descriptor.MediaType == MediaType.Unknown)
            {
                return new UrlParseResult(UrlParseStatus.NoMedia);
            }

            return new UrlParseResult
            {
                ParseStatus = UrlParseStatus.Success,
                MediaDescriptor = descriptor
            };
        }

        /// <summary>
        /// Resolves the media descriptor to a media collection object.
        /// </summary>
        /// <returns>An <see cref="IMediaCollection"/> according to the <see cref="MediaDescriptor.MediaType"/> property.</returns>
        public async Task<IMediaCollection> ResolveMedia(MediaDescriptor descriptor)
        {
            var mediaId = descriptor.MediaId;
            var service = MediaServiceManager.GetService(descriptor.OriginalUri);
            return descriptor.MediaType switch
            {
                MediaType.Album => await service.GetAlbumAsync(mediaId, withTracks: true).ConfigureAwait(false),
                MediaType.Track => (await service.GetTrackAsync(mediaId).ConfigureAwait(false)).AsCollection(),
                MediaType.Playlist => await service.GetPlaylistAsync(mediaId).ConfigureAwait(false),
                _ => throw new InvalidOperationException("Can't resolve unknown media type")
            };
        }
    }
}
