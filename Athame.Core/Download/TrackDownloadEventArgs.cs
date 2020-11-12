using Athame.Plugin.Api.Downloader;

namespace Athame.Core.Download
{
    public class TrackDownloadEventArgs
    {
        public TrackFile TrackFile { get; set; }

        public TrackStatus Status { get; set; }

        public int PercentCompleted { get; set; }

        public void PostUpdate(ProgressInfo progress)
        {
            PercentCompleted = progress.PercentCompleted;
            
            Status = PercentCompleted == 100 
                ? TrackStatus.PostProcess 
                : TrackStatus.DownloadingTrack;
        }
    }
}
