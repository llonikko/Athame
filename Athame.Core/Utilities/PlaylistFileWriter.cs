using Athame.Plugin.Api.Downloader;
using Athame.Plugin.Api.Service;
using System.Collections.Generic;
using System.IO;

namespace Athame.Core.Utilities
{
    public class PlaylistFileWriter
    {
        private readonly PlaylistFile playlistFile;

        public PlaylistFileWriter(PlaylistFile playlistFile)
        {
            this.playlistFile = playlistFile;
        }

        public void Write(string fileName, IEnumerable<TrackFile> trackFiles)
        {
            playlistFile.BuildContent(trackFiles);
            FileInfo file = new FileInfo($"{fileName}.{playlistFile.Extension}");
            if (!file.Exists)
            {
                var writer = file.CreateText();
                writer.WriteLine(playlistFile.Content);
                writer.Close();
            }
        }
    }

    public class MediaInfoWriter
    {
        private readonly MediaInfo mediaInfo;

        public MediaInfoWriter(MediaInfo mediaInfo)
        {
            this.mediaInfo = mediaInfo;
        }

        public void Write(string fileName, IMedia media)
        {
            mediaInfo.BuildContent(media);
            FileInfo file = new FileInfo($"{fileName}.{mediaInfo.Extension}");
            if (!file.Exists)
            {
                var writer = file.CreateText();
                writer.WriteLine(mediaInfo.Content);
                writer.Close();
            }
        }
    }
}
