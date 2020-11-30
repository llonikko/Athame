using Athame.Core.Download;
using Athame.Core.Extensions;
using Athame.Core.Interface;
using Athame.Plugin.Api.Interface;
using Athame.Plugin.Api.Service;
using System;
using System.Threading.Tasks;

namespace Athame.Core.Search
{
    public class MediaResolver
    {
        private readonly MediaDescriptor descriptor;

        public MediaResolver(MediaDescriptor descriptor)
        {
            this.descriptor = descriptor;
        }

        /// <summary>
        /// Resolves the media descriptor to a media item object.
        /// </summary>
        /// <returns>An <see cref="MediaItem"/> according to the <see cref="MediaDescriptor.MediaType"/> property.</returns>
        public async Task<MediaItem> ResolveMedia()
        {
            var id = descriptor.MediaId;
            var service = AthameApp.GetService(descriptor.OriginalUri);

            ITrackCollection media = descriptor.MediaType switch
            {
                MediaType.Album
                    => await service.GetAlbumAsync(id, withTracks: true),
                MediaType.Track
                    => (await service.GetTrackAsync(id)).AsCollection(),
                MediaType.Playlist
                    => await service.GetPlaylistAsync(id),
                _ => throw new InvalidOperationException("Can't resolve unknown media type")
            };

            return new MediaItem(media, descriptor);
        }

        public static MediaResolver Create(IUrlParseResult result)
            => new MediaResolver(result.AsMediaUrlParseResult().MediaDescriptor);
    }
}
