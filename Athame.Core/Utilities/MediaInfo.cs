using Athame.Plugin.Api.Interface;
using Athame.Plugin.Api.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Athame.Core.Utilities
{
    public abstract class MediaInfo : ContentInfo
    {
        public override string Extension => "txt";

        protected abstract void BuildInfo(StringBuilder content, IMedia media);

        protected void BuildTrackListTextInfo(StringBuilder content, IEnumerable<Track> tracks)
        {
            string format = tracks.Count() > 99 ? "{0:000}" : "{0:00}";
            foreach (var track in tracks)
            {
                content.Append(string.Format(format, track.TrackNumber));
                content.Append(" - ");
                content.Append($"{track.Title}");
                content.AppendLine();
            }
        }

        public MediaInfo BuildContent(IMedia media)
        {
            BuildInfo(content, media);
            return this;
        }

        public static MediaInfo Create(MediaType mediaType)
            => mediaType switch
            {
                MediaType.Album    => new AlbumInfo(),
                MediaType.Playlist => new PlaylistInfo(),
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}
