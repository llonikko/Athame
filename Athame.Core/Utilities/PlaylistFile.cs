using Athame.Plugin.Api.Downloader;
using Athame.Plugin.Api.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace Athame.Core.Utilities
{
    public abstract class PlaylistFile : ITextInfo
    {
        private readonly StringBuilder contentBuilder = new StringBuilder();

        public virtual string Name { get; }
        public virtual string Extension { get; }
        public string Content 
            => contentBuilder.ToString();

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
            Initialize(contentBuilder);
            BuildHeader(contentBuilder, trackFiles);
            BuildEntries(contentBuilder, trackFiles);
            BuildFooter(contentBuilder, trackFiles);
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
