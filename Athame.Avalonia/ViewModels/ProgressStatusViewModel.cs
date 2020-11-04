using Athame.Core.Extensions;
using Athame.Plugin.Api.Downloader;
using ReactiveUI.Fody.Helpers;

namespace Athame.Avalonia.ViewModels
{
    public class ProgressStatusViewModel : ViewModelBase
    {
        [Reactive]
        public string MediaDownloadStatus { get; set; }
        [Reactive]
        public string TrackDownloadStatus { get; set; }
        [Reactive]
        public int TrackDownloadProgressPercentage { get; set; }
        [Reactive]
        public string TrackDownloadTitle { get; set; }

        public ProgressStatusViewModel()
        {
            MediaDownloadStatus = "No media";
            TrackDownloadStatus = TrackStatus.Ready.GetDescription();
            TrackDownloadProgressPercentage = 0;
        }
    }
}
