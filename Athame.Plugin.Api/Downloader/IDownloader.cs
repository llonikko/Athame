using System;
using System.Threading.Tasks;

namespace Athame.Plugin.Api.Downloader
{
    public interface IDownloader
    {
        Task DownloadAsync(IDownloadable file, IProgress<ProgressInfo> progress);
    }
}
