using System;

namespace Athame.Core.Download
{
    public class MediaDownloadEventArgs : EventArgs
    {
        public MediaDownloadService Service { get; set; }
        public int Index { get; set; }
        public int Total { get; set; }
    }
}
