using Athame.Core.Extensions;
using Athame.Plugin.Api.Downloader;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Athame.Core.Utilities
{
    public class M3UFile : PlaylistFile
    {
        public override string Extension => "m3u8";

        protected override void BuildHeader(StringBuilder content, IEnumerable<TrackFile> trackFiles)
        {
            content.AppendLine("#EXTM3U");
            content.AppendLine();
        }

        protected override void BuildEntries(StringBuilder content, IEnumerable<TrackFile> trackFiles)
        {
            foreach (var trackFile in trackFiles)
            {
                var duration = trackFile.Track.TotalSeconds;
                content.AppendLine($"#EXTINF:{duration},{trackFile.Track.Artist} - {trackFile.Track.Title}");
                content.Append(".");
                content.Append(Path.DirectorySeparatorChar);
                content.AppendLine(trackFile.RelativePath);
                content.AppendLine();
            }
        }
    }
}
