using System;

namespace Athame.Plugin.Api.Downloader
{
    public class DownloadEventArgs : EventArgs
    {
        public int PercentCompleted { get; set; }
    }
}
