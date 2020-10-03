using Athame.Plugin.Api.Downloader;
using System;

namespace Athame.Plugin.Api.Service
{
    /// <summary>
    /// Represents a picture for a media item.
    /// </summary>
    public class MediaCover
    {
        /// <summary>
        /// The file type of the picture.
        /// </summary>
        public FileType FileType { get; set; }

        public string Id { get; set; }

        public byte[] Data { get; set; }
    }
}
