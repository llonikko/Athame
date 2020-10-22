using System;
using System.Threading.Tasks;

namespace Athame.Plugin.Api.Downloader
{
    public class TrackDownloader : IDownloader
    {
        protected readonly HttpDownloader downloader;

        public TrackDownloader(HttpDownloader downloader)
        {
            this.downloader = downloader;
        }

        public virtual async Task DownloadAsync(IDownloadable file, IProgress<ProgressInfo> progress)
        {
            await downloader.DownloadFileAsync(file.DownloadUri, file.FullPath, progress).ConfigureAwait(false);
        }
    }
}