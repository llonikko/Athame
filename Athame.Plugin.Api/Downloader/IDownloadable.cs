using System;

namespace Athame.Plugin.Api.Downloader
{
    public interface IDownloadable
    {
        /// <summary>
        /// The URI to the file.
        /// </summary>
        Uri DownloadUri { get; set; }

        /// <summary>
        /// The full path of the file
        /// </summary>
        string FullPath { get; set; }

        bool IsDownloadable { get; }
    }
}
