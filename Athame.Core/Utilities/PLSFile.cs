using Athame.Core.Extensions;
using Athame.Plugin.Api.Downloader;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Athame.Core.Utilities
{
    public class PLSFile : PlaylistFile
    {
        public override string Extension => "pls";

        protected override void BuildHeader(StringBuilder content, IEnumerable<TrackFile> trackFiles)
        {
            content.AppendLine("[playlist]");
            content.AppendLine();
        }

        protected override void BuildEntries(StringBuilder content, IEnumerable<TrackFile> trackFiles)
        {
            var fileCount = 0;
            foreach (var trackFile in trackFiles)
            {
                fileCount++;
                content.AppendLine($"File{fileCount}=.{Path.DirectorySeparatorChar}{trackFile.RelativePath}");
                var duration = trackFile.Track.TotalSeconds;
                if (duration > 0)
                {
                    content.AppendLine($"Length{fileCount}={duration}");
                }
                content.AppendLine($"Title{fileCount}={trackFile.Track.Title}");
                content.AppendLine();
            }
        }

        protected override void BuildFooter(StringBuilder content, IEnumerable<TrackFile> trackFiles)
        {
            content.AppendLine($"NumberOfEntries={trackFiles.Count()}");
            content.AppendLine("Version=2");
        }
    }
}
