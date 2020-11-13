using Athame.Core.Settings;
using Athame.Core.Utilities;
using Athame.Plugin.Api.Downloader;
using Athame.Plugin.Api.Interface;
using Athame.Plugin.Api.Service;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Athame.Core.Download
{
    public class MediaDownloadContext
    {
        private readonly string root;
        private string pathFormat;

        public string MediaFolder { get; private set; }
        public ICollection<TrackFile> TrackFiles { get; } = new List<TrackFile>();

        public MediaDownloadContext(MediaPreference preference)
        {
            root = preference.Location;
            pathFormat = preference.PathFormat;
        }

        public void CreateMediaFolder(ITrackCollection media)
        {
            var parts = PathHelpers.Split(pathFormat).ToList();
            if (media is Album album && album.NumberOfDiscs > 1)
            {
                parts.Add(parts[^1]);
                parts[^2] = "CD{DiscNumber:0}";

                pathFormat = PathHelpers.Join(parts.ToArray());

                CreateMediaFolder(album.Tracks.First(), PathHelpers.Join(parts.ToArray()[..^2]));

                for (var disc = 1; disc <= album.NumberOfDiscs; disc++)
                {
                    PathHelpers.CreateFolder(Path.Combine(MediaFolder, $"CD{disc}"));
                }
            }
            else
            {
                CreateMediaFolder(media.Tracks.First(), PathHelpers.Join(parts.ToArray()[..^1]));
            }
        }

        private void CreateMediaFolder(Track track, string format)
        {
            MediaFolder = Path.Combine(root, PathHelpers.FormatFilePath(track, format));
            PathHelpers.CreateFolder(MediaFolder);
        }

        public void CreatePath(TrackFile trackFile)
            => trackFile.FullPath = CreateFullPath(trackFile);

        public string CreateFullPath(TrackFile trackFile)
            => Path.Combine(root, CreateFileName(trackFile, pathFormat));

        public string CreateFileName(TrackFile trackFile, string format)
            => trackFile.FileType.AppendExtension(PathHelpers.FormatFilePath(trackFile.Track, format));
    }
}
