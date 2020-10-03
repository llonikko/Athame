using Athame.Plugin.Api.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Athame.Core.Utilities
{
    public abstract class MediaInfo : ITextInfo
    {
        private readonly StringBuilder contentBuilder = new StringBuilder();

        public virtual string Name { get; set; }
        public virtual string Extension => "txt";
        public string Content
            => contentBuilder.ToString();

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
            BuildInfo(contentBuilder, media);
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
