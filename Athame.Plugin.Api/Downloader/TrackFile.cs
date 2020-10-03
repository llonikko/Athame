using Athame.Plugin.Api.Service;
using System;
using System.IO;

namespace Athame.Plugin.Api.Downloader
{
    /// <summary>
    /// Represents the downloadable form of <see cref="Track"/>.
    /// </summary>
    public class TrackFile : IDownloadable
    {
        /// <summary>
        /// The track this file references.
        /// </summary>
        public Track Track { get; set; }

        /// <summary>
        /// The full path of the track.
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// The filename of the track.
        /// </summary>
        public string FileName 
            => Path.GetFileName(FullPath);

        /// <summary>
        /// The file type. This should be set using the MIME type of the file.
        /// </summary>
        public FileType FileType { get; set; }

        public Uri DownloadUri { get; set; }

        public bool IsDownloadable
            => Track.IsDownloadable;

        public string RelativePath
            => Track.Album.NumberOfDiscs > 1
                ? Path.Combine(Directory.GetParent(FullPath).Name, FileName)
                : FileName;
    }
}
