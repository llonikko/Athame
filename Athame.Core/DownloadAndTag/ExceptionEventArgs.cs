using System;

namespace Athame.Core.DownloadAndTag
{
    /// <summary>
    /// Event args used when an exception occurs while downloading a track.
    /// </summary>
    public class ExceptionEventArgs : EventArgs
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
        public ExceptionSkip SkipTo { get; set; }
    }
}