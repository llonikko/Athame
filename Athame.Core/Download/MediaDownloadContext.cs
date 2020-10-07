using Athame.Core.Utilities;
using Athame.Plugin.Api.Downloader;
using Athame.Plugin.Api.Service;
using System.IO;
using System.Linq;

namespace Athame.Core.Download
{
    public class MediaDownloadContext
    {
        public string DownloadFolderPath { get; set; }
        public string MediaPathFormat { get; set; }

        public string MediaFolderPath { get; private set; }
        
        public void CreateMediaFolder(IMediaCollection media)
        {
            var parts = PathHelpers.Split(MediaPathFormat).ToList();
            if (media is Album album && album.NumberOfDiscs > 1)
            {
                parts.Add(parts[^1]);
                parts[^2] = "CD{DiscNumber:0}";

                MediaPathFormat = PathHelpers.Join(parts.ToArray());

                CreateMediaFolder(album.Tracks.First(), PathHelpers.Join(parts.ToArray()[..^2]));

                for (var disc = 1; disc <= album.NumberOfDiscs; disc++)
                {
                    PathHelpers.CreateFolder(Path.Combine(MediaFolderPath, $"CD{disc}"));
                }
            }
            else
            {
                CreateMediaFolder(media.Tracks.First(), PathHelpers.Join(parts.ToArray()[..^1]));
            }
        }

        private void CreateMediaFolder(Track track, string format)
        {
            MediaFolderPath = Path.Combine(DownloadFolderPath, PathHelpers.FormatFilePath(track, format));
            PathHelpers.CreateFolder(MediaFolderPath);
        }

        public string CreateFilePath(TrackFile trackFile, string format)
            => trackFile.FileType.AppendExtension(PathHelpers.FormatFilePath(trackFile.Track, format));

        public string CreateFullPath(TrackFile trackFile)
            => Path.Combine(DownloadFolderPath, CreateFilePath(trackFile, MediaPathFormat));
    }
}
