using Athame.Plugin.Api.Downloader;
using Athame.Plugin.Api.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace Athame.Core.Utilities
{
    public abstract class PlaylistFile : ContentInfo
    {
        protected virtual void Initialize(StringBuilder content)
        {
            content.Clear();
        }

        protected virtual void BuildHeader(StringBuilder content, IEnumerable<TrackFile> trackFiles)
        {
        }

        protected virtual void BuildEntries(StringBuilder content, IEnumerable<TrackFile> trackFiles)
        {
        }

        protected virtual void BuildFooter(StringBuilder content, IEnumerable<TrackFile> trackFiles)
        {
        }

        public PlaylistFile BuildContent(IEnumerable<TrackFile> trackFiles)
        {
            Initialize(content);
            BuildHeader(content, trackFiles);
            BuildEntries(content, trackFiles);
            BuildFooter(content, trackFiles);
            return this;
        }

        public static PlaylistFile Create(PlaylistFileType type)
           => type switch
           {
               PlaylistFileType.M3U => new M3UFile(),
               PlaylistFileType.PLS => new PLSFile(),
               _ => throw new InvalidOperationException()
           };
    }
}
