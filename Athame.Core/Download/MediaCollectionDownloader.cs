using Athame.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Athame.Core.Download
{
    public class MediaCollectionDownloader : IMediaCollectionDownloader
    {
        private readonly ITrackCollectionDownloader downloader;

        public MediaCollectionDownloader(ITrackCollectionDownloader downloader)
        {
            this.downloader = downloader;
        }

        public async Task DownloadAsync(IEnumerable<MediaItem> items)
        {
            var mediaQ = new Queue<MediaItem>(items);
            while (mediaQ.Count > 0)
            {
                var currentMedia = mediaQ.Dequeue();
                var e = new MediaDownloadEventArgs
                {
                    CurrentMediaDownload = currentMedia,
                    Index = items.Count() - mediaQ.Count - 1,
                    Total = items.Count()
                };

                OnMediaDownloadStarted(e);

                await downloader.DownloadAsync(currentMedia);

                OnMediaDownloadCompleted(e);
            }
        }

        #region Events
        public event EventHandler<MediaDownloadEventArgs> MediaDownloadStarted;
        protected void OnMediaDownloadStarted(MediaDownloadEventArgs e)
            => MediaDownloadStarted?.Invoke(this, e);

        public event EventHandler<MediaDownloadEventArgs> MediaDownloadCompleted;
        protected void OnMediaDownloadCompleted(MediaDownloadEventArgs e)
            => MediaDownloadCompleted?.Invoke(this, e);
        #endregion
    }
}
