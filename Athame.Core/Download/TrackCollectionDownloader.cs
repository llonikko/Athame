using Athame.Core.Interface;
using Athame.Plugin.Api.Downloader;
using Athame.Plugin.Api.Service;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Athame.Core.Download
{
    public class TrackCollectionDownloader : ITrackCollectionDownloader
    {
        private readonly TrackDownloadEventArgs e = new TrackDownloadEventArgs();

        public MediaDownloadContext Context { get; set; }
        public IMediaService MediaService { get; set; }

        public async Task DownloadAsync(MediaItem item)
        {
            foreach (var track in item.Media.Tracks)
            {
                try
                {
                    e.TrackFile = await MediaService.GetDownloadableTrackAsync(track);
                    e.PercentCompleted = 0;
                    e.Status = TrackStatus.PreProcess;

                    OnTrackDownloadStarted(e);

                    InitializeOperation();
                    if (TrackFile.Exists(e.TrackFile) || !e.TrackFile.Track.IsDownloadable)
                    {
                        e.Status = TrackStatus.DownloadSkipped;
                        OnTrackDownloadCompleted(e);
                        continue;
                    }

                    await DownloadArtworkAsync();
                    await DownloadTrackAsync();
                }
                catch (Exception ex)
                {
                    e.Status = TrackStatus.DownloadFailed;
                    Log.Error(ex, "");
                }

                OnTrackDownloadCompleted(e);
            }
        }

        private void InitializeOperation()
        {
            Context.CreatePath(e.TrackFile);
            Context.TrackFiles.Add(e.TrackFile);
        }

        private async Task DownloadArtworkAsync()
        {
            e.Status = TrackStatus.DownloadingArtwork;
            OnTrackDownloadProgressChanged(e);

            var media = e.TrackFile.Track.Album;
            if (!ImageCache.Instance.HasImage(media)) // Download artwork if it's not cached
            {
                try
                {
                    var img = await MediaService.GetMediaCoverAsync(media.Cover.Id);
                    ImageCache.Instance.AddEntry(img);
                }
                catch (Exception ex)
                {
                    ImageCache.Instance.AddNull(media);
                    Log.Warning(ex, "{Service}: Exception occurred when download album artwork.", MediaService.Name);
                }
            }
        }

        private async Task DownloadTrackAsync()
        {
            e.Status = TrackStatus.DownloadingTrack;
            try
            {
                var progress = new Progress<ProgressInfo>(info =>
                {
                    e.PercentCompleted = info.PercentCompleted;
                    OnTrackDownloadProgressChanged(e);
                });
                await MediaService.GetDownloader().DownloadAsync(e.TrackFile, progress);
                e.Status = TrackStatus.DownloadCompleted;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{Service}: Track download failed.", MediaService.Name);
                e.Status = TrackStatus.DownloadFailed;
            }
        }

        #region Events
        public event EventHandler<TrackDownloadEventArgs> TrackDownloadStarted;
        protected void OnTrackDownloadStarted(TrackDownloadEventArgs e)
            => TrackDownloadStarted?.Invoke(this, e);

        public event EventHandler<TrackDownloadEventArgs> TrackDownloadCompleted;
        protected void OnTrackDownloadCompleted(TrackDownloadEventArgs e)
            => TrackDownloadCompleted?.Invoke(this, e);

        public event EventHandler<TrackDownloadEventArgs> TrackDownloadProgressChanged;
        protected void OnTrackDownloadProgressChanged(TrackDownloadEventArgs e)
            => TrackDownloadProgressChanged?.Invoke(this, e);

        public event EventHandler<TrackDownloadEventArgs> TrackDownloadFailed;
        protected void OnTrackDownloadFailed(TrackDownloadEventArgs e)
            => TrackDownloadFailed?.Invoke(this, e);

        public event EventHandler<TrackDownloadEventArgs> TrackDownloadSkipped;
        protected void OnTrackDownloadSkipped(TrackDownloadEventArgs e)
            => TrackDownloadSkipped?.Invoke(this, e);
        #endregion
    }
}
