using Athame.Plugin.Api.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Athame.Plugin.Api.Downloader
{
    /// <summary>
    /// The default implementation of <see cref="IDownloader"/>
    /// </summary>
    public class TrackDownloader : IDownloader
    {
        protected readonly HttpDownloader downloader;
        protected CancellationTokenSource cancellationSource;

        public TrackDownloader(HttpDownloader downloader)
        {
            this.downloader = downloader;
        }

        public virtual async Task DownloadAsync(IDownloadable file, IProgress<ProgressInfo> progress)
        {
            cancellationSource = new CancellationTokenSource();

            await downloader
                .DownloadFileAsync(file.DownloadUri, file.FullPath, progress, cancellationSource.Token)
                .ConfigureAwait(false);
            
            cancellationSource.Dispose();
        }

        public virtual void Cancel()
        {
            cancellationSource.Cancel();
            cancellationSource.Dispose();
        }
    }
}
