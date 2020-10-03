using Athame.Plugin.Api.Downloader;

namespace Athame.Core.Download
{
    public class TrackDownloadEventArgs : DownloadEventArgs
    {
        public TrackFile TrackFile { get; set; }

        public int Total { get; set; }
        public int CurrentItemIndex { get; set; }

        public void PostUpdate(ProgressInfo progress)
        {
            PercentCompleted = progress.PercentCompleted;
            
            DownloadState = PercentCompleted == 100 
                ? DownloadState.PostProcess 
                : DownloadState.DownloadingTrack;
        }
    }
}
