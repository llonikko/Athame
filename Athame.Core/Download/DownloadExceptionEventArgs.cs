using System;

namespace Athame.Core.Download
{
    /// <summary>
    /// Event args used when an exception occurs while downloading a track.
    /// </summary>
    public class DownloadExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// The exception that occurred
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// The current state of the item being downloaded.
        /// </summary>
        public TrackDownloadEventArgs CurrentState { get; set; }

        /// <summary>
        /// What the downloader should advance to when the event handler returns.
        /// </summary>
        public DownloadSkip SkipTo { get; set; }
    }
}
