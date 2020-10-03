using System;

namespace Athame.Plugin.Api.Downloader
{
    public class DownloadEventArgs : EventArgs
    {
        public DownloadState DownloadState { get; set; }
        public int PercentCompleted { get; set; }
    }
}
