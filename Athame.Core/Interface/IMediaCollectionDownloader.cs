using Athame.Core.Download;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Athame.Core.Interface
{
    public interface IMediaCollectionDownloader
    {
        Task DownloadAsync(IEnumerable<MediaItem> items);

        /// <summary>
        /// Raised when a media collection download is started.
        /// </summary>
        public event EventHandler<MediaDownloadEventArgs> MediaDownloadStarted;

        /// <summary>
        /// Raised when a media collection download is completed.
        /// </summary>
        public event EventHandler<MediaDownloadEventArgs> MediaDownloadCompleted;
    }
}
