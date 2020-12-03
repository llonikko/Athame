using Athame.Plugin.Api.Interface;
using Athame.Plugin.Api.Service;
using System;
using System.Text;

namespace Athame.Core.Utilities
{
    public abstract class MediaInfo : ContentInfo
    {
        public override string Extension => "txt";

        protected abstract void BuildInfo(StringBuilder content, IMedia media);

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
