using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Athame.Plugin.Api.Downloader
{
    public class HttpDownloader
    {
        private readonly HttpClient http;
        private readonly ProgressData progressData;

        private const int DefaultDownloadBufferLength = 65536;

        public HttpDownloader() 
            : this(new HttpClient()) 
        {
        }

        public HttpDownloader(HttpClient http)
        {
            this.http = http;
            progressData = new ProgressData();
        }
        
        public Task<byte[]> DownloadImageDataAsync(string uri)
            => http.GetByteArrayAsync(uri);

        public Task DownloadFileAsync(string uri, string destination, IProgress<ProgressInfo> progress, CancellationToken ct) 
            => DownloadFileAsync(new Uri(uri), destination, progress, ct);

        public async Task DownloadFileAsync(Uri uri, string destination, IProgress<ProgressInfo> progress, CancellationToken ct)
        {
            var response = await http.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            Initialize(response);

            using var content = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            await DownloadBytesAsync(content, destination, progress, ct).ConfigureAwait(false);
            UpdateProgress(progress);
        }

        private void Initialize(HttpResponseMessage response)
        {
            progressData.Reset();
            long contentLength = (long)response.Content.Headers.ContentLength;
            if (contentLength >= 0)
            {
                progressData.TotalBytesToReceive = contentLength;
            }
        }

        private async Task DownloadBytesAsync(Stream content, string destination, IProgress<ProgressInfo> progress, CancellationToken ct)
        {
            byte[] buffer = new byte[DefaultDownloadBufferLength];
            using var file = File.OpenWrite(destination);

            while (true)
            {
                var bytesRead = await content.ReadAsync(new Memory<byte>(buffer), ct).ConfigureAwait(false);
                if (bytesRead == 0)
                {
                    break;
                }
                UpdateProgress(bytesRead, progress);

                await file.WriteAsync(new ReadOnlyMemory<byte>(buffer, 0, bytesRead), ct).ConfigureAwait(false);
            }
        }

        private void UpdateProgress(int bytesRead, IProgress<ProgressInfo> progress)
        {
            progressData.BytesReceived += bytesRead;
            if (progressData.BytesReceived != progressData.TotalBytesToReceive)
            {
                PostProgressChanged(progress);
            }
        }

        private void UpdateProgress(IProgress<ProgressInfo> progress)
        {
            if (progressData.TotalBytesToReceive < 0)
            {
                progressData.TotalBytesToReceive = progressData.BytesReceived;
            }
            PostProgressChanged(progress);
        }

        private void PostProgressChanged(IProgress<ProgressInfo> progress)
        {
            if (progress != null)
            {
                progress.Report(new ProgressInfo
                {
                    BytesReceived = progressData.BytesReceived,
                    TotalBytesToReceive = progressData.TotalBytesToReceive
                });
            }
        }

        private sealed class ProgressData
        {
            internal long BytesReceived = 0;
            internal long TotalBytesToReceive = -1;

            internal void Reset()
            {
                BytesReceived = 0;
                TotalBytesToReceive = -1;
            }
        }
    }
}
