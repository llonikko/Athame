using Athame.Core.Download;
using System;
using System.Threading.Tasks;

namespace Athame.Core.Interface
{
    public interface ITrackCollectionDownloader
    {
        Task DownloadAsync(MediaItem item);

        /// <summary>
        /// Raised when a track download is started.
        /// </summary>
        event EventHandler<TrackDownloadEventArgs> TrackDownloadStarted;

        /// <summary>
        /// Raised when a track download is completed.
        /// </summary>
        event EventHandler<TrackDownloadEventArgs> TrackDownloadCompleted;

        /// <summary>
        /// Raised when a track download progress changes.
        /// </summary>
        event EventHandler<TrackDownloadEventArgs> TrackDownloadProgressChanged;

        /// <summary>
        /// Raised when a track download fails.
        /// </summary>
        event EventHandler<TrackDownloadEventArgs> TrackDownloadFailed;

        /// <summary>
        /// Raised when a track download is skipped.
        /// </summary>
        event EventHandler<TrackDownloadEventArgs> TrackDownloadSkipped;
    }
}
