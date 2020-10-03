using Athame.Core.Extensions;
using Athame.Core.Settings;
using Athame.Core.Utilities;
using Athame.Plugin.Api.Downloader;
using Athame.Plugin.Api.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Athame.Core.Download
{
    public class MediaDownloader
    {
        private DownloadSkip skip;
       
        public bool CanWriteWatermark { get; set; }
        public bool DontSavePlaylistArtwork { get; set; }
        public PlaylistFileType PlaylistFileType { get; set; }
        public ArtworkFileName ArtworkFileName { get; set; }

        public async Task DownloadMediaAsync(IEnumerable<MediaDownloadService> services)
        {
            var serviceQ = new Queue<MediaDownloadService>(services);
            while (serviceQ.Count > 0)
            {
                var service = serviceQ.Dequeue();
                OnMediaDownloadStarted(new MediaDownloadEventArgs
                {
                    Service = service,
                    Index = services.Count() - serviceQ.Count - 1,
                    Total = services.Count()
                });

                service.CreateMediaFolder();
                if (await DownloadMediaAsync(service))
                {
                    continue;
                }
                
                if (skip == DownloadSkip.Fail) return;
            }
        }

        private async Task<bool> DownloadMediaAsync(MediaDownloadService service)
        {
            var tracksTotal = service.Media.Tracks.Count;
            var tracksQueue = new Queue<Track>(service.Media.Tracks);

            while (tracksQueue.Count > 0)
            {
                var track = tracksQueue.Dequeue();
                var trackFile = await service.CreateTrackFile(track);

                var e = new TrackDownloadEventArgs
                {
                    TrackFile = trackFile,
                    CurrentItemIndex = (tracksTotal - tracksQueue.Count),
                    PercentCompleted = 0,
                    DownloadState = DownloadState.PreProcess,
                    Total = service.Media.GetDownloadableTracksCount()
                };

                if (await DownloadTrackAsync(service, e))
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }

            service.CreateArtworkFile(ArtworkFileName);
            service.CreateMediaInfo();
            service.CreatePlaylistFile(PlaylistFileType);

            return true;
        }

        private async Task<bool> DownloadTrackAsync(MediaDownloadService service, TrackDownloadEventArgs e)
        {
            if (!e.TrackFile.Track.IsDownloadable)
            {
                OnTrackDownloadSkipped(e);
            }
            else
            {
                OnTrackDownloadStarted(e);

                e.DownloadState = DownloadState.DownloadingAlbumArtwork;
                OnTrackDownloadProgressed(e);
                await service.DownloadArtworkAsync(e.TrackFile.Track.Album);
                
                try
                {
                    var progress = new Progress<ProgressInfo>(progress =>
                    {
                        e.PostUpdate(progress);
                        OnTrackDownloadProgressed(e);
                    });
                    await service.MediaService
                        .GetDownloader()
                        .DownloadAsync(e.TrackFile, progress);

                    e.DownloadState = DownloadState.WritingTags;
                    OnTrackDownloadProgressed(e);
                    TrackTagger.Tag(e.TrackFile, CanWriteWatermark);
                }
                catch (Exception ex)
                {
                    if (!HandleDownloadException(e, ex))
                    {
                        return false;
                    }
                    return true;
                }

                OnTrackDownloadCompleted(e); // Raise the completed event even if an error occurred
            }
            return true;
        }

        private void WriteMediaArtwork(MediaDownloadService mediaService)
        {
            var setting = ArtworkFileName;
            if (DontSavePlaylistArtwork && mediaService.Media.MediaType is MediaType.Playlist)
            {
                //setting = MediaArtworkType.DontSave;
                return;
            }
        }

        private bool HandleDownloadException(TrackDownloadEventArgs e, Exception ex)
        {
            var dlex = new DownloadExceptionEventArgs
            {
                CurrentState = e,
                Exception = ex
            };
            OnException(dlex);
            switch (dlex.SkipTo)
            {
                case DownloadSkip.NextItem: // continue
                    break;
                case DownloadSkip.NextCollection:
                case DownloadSkip.Fail:
                    skip = dlex.SkipTo;
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return true;
        }

        public void ApplySettings(AthameSettings settings)
        {
            PlaylistFileType = settings.PlaylistFileType;
            ArtworkFileName = settings.ArtworkFileName;
            DontSavePlaylistArtwork = settings.DontSavePlaylistArtwork;
            CanWriteWatermark = settings.WriteWatermark;
        }

        #region Events
        /// <summary>
        /// Raised when a media collection download is started.
        /// </summary>
        public event EventHandler<MediaDownloadEventArgs> MediaDownloadStarted;
        protected void OnMediaDownloadStarted(MediaDownloadEventArgs e)
            => MediaDownloadStarted?.Invoke(this, e);

        /// <summary>
        /// Raised when a track download is started.
        /// </summary>
        public event EventHandler<TrackDownloadEventArgs> TrackDownloadStarted;
        protected void OnTrackDownloadStarted(TrackDownloadEventArgs e)
            => TrackDownloadStarted?.Invoke(this, e);

        /// <summary>
        /// Raised when a track download is completed.
        /// </summary>
        public event EventHandler<TrackDownloadEventArgs> TrackDownloadCompleted;
        protected void OnTrackDownloadCompleted(TrackDownloadEventArgs e)
            => TrackDownloadCompleted?.Invoke(this, e);

        /// <summary>
        /// Raised when a track download progress changes.
        /// </summary>
        public event EventHandler<TrackDownloadEventArgs> TrackDownloadProgressed;
        protected void OnTrackDownloadProgressed(TrackDownloadEventArgs e)
            => TrackDownloadProgressed?.Invoke(this, e);

        public event EventHandler<DownloadExceptionEventArgs> Exception;
        protected void OnException(DownloadExceptionEventArgs e)
            => Exception?.Invoke(this, e);

        /// <summary>
        /// Raised when a track download is skipped.
        /// </summary>
        public event EventHandler<TrackDownloadEventArgs> TrackDownloadSkipped;
        protected void OnTrackDownloadSkipped(TrackDownloadEventArgs e)
            => TrackDownloadSkipped?.Invoke(this, e);
        #endregion
    }
}
