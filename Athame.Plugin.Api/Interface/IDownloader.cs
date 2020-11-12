using Athame.Plugin.Api.Downloader;
using System;
using System.Threading.Tasks;

namespace Athame.Plugin.Api.Interface
{
    public interface IDownloader
    {
        Task DownloadAsync(IDownloadable file, IProgress<ProgressInfo> progress);
    }
}
