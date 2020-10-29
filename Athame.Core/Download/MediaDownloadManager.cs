using Athame.Core.Extensions;
using Athame.Core.Plugin;
using Athame.Core.Settings;
using Athame.Core.Utilities;
using Athame.Plugin.Api.Downloader;
using Athame.Plugin.Api.Service;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Athame.Core.Download
{
    public class MediaDownloadManager
    {
        private IMediaService service;

        public AthameSettings Settings { get; set; }

        public async Task StartDownloadAsync(IEnumerable<MediaDownloadItem> items)
        {
            var itemsQ = new Queue<MediaDownloadItem>(items);
            while (itemsQ.Count > 0)
            {
                var item = itemsQ.Dequeue();
                item.CreateMediaFolder();

                service = MediaServiceManager.GetService(item.Descriptor.OriginalUri);

                var e = new MediaDownloadEventArgs
                {
                    Item = item,
                    Index = items.Count() - itemsQ.Count - 1,
                    Total = items.Count()
                };
                OnMediaDownloadStarted(e);

                await DownloadTrackListAsync(item);

                CreateArtworkFile(item);
                CreateMediaInfo(item);
                CreatePlaylistFile(item);
            }
        }

        private async Task DownloadTrackListAsync(MediaDownloadItem item)
        {
            foreach (var track in item.Media.Tracks)
            {
                var trackFile = await service.GetDownloadableTrackAsync(track);

                item.CreatePath(trackFile);
                item.TrackFiles.Add(trackFile);

                if (TrackFile.Exists(trackFile) || !trackFile.Track.IsDownloadable)
                {
                    TrackDownloadSkip(trackFile);
                    continue;
                }

                TrackDownloadStart(trackFile);

                await DownloadArtworkAsync(trackFile);

                var status = await DownloadTrackAsync(trackFile);
                if (status == TrackStatus.DownloadCompleted)
                {
                    TrackDownloadComplete(trackFile);
                }
                else
                {
                    TrackDownloadFail(trackFile);
                }
            }
        }

        private void TrackDownloadSkip(TrackFile trackFile)
        {
            var e = new TrackDownloadEventArgs
            {
                TrackFile = trackFile,
                PercentCompleted = 0,
                Status = TrackStatus.SkippingTrack
            };
            OnTrackDownloadSkipped(e);
        }

        private void TrackDownloadStart(TrackFile trackFile)
        {
            var e = new TrackDownloadEventArgs
            {
                TrackFile = trackFile,
                PercentCompleted = 0,
                Status = TrackStatus.PreProcess
            };
            OnTrackDownloadStarted(e);
        }

        private async Task DownloadArtworkAsync(TrackFile trackFile)
        {
            var e = new TrackDownloadEventArgs
            {
                TrackFile = trackFile,
                PercentCompleted = 0,
                Status = TrackStatus.DownloadingArtwork
            };
            OnTrackDownloadProgressed(e);

            await DownloadArtworkAsync(e.TrackFile.Track.Album);
        }

        public async Task DownloadArtworkAsync(IMedia media)
        {
            if (!ImageCache.Instance.HasImage(media)) // Download artwork if it's not cached
            {
                try
                {
                    var img = await service.GetMediaCoverAsync(media.Cover.Id);
                    ImageCache.Instance.AddEntry(img);
                }
                catch (Exception ex)
                {
                    ImageCache.Instance.AddNull(media);
                    Log.Warning(ex, "{Service}: Exception occurred when download album artwork.", service.Name);
                }
            }
        }

        private void TrackDownloadComplete(TrackFile trackFile)
        {
            TrackTagger.Tag(trackFile, Settings.WriteWatermark);

            var e = new TrackDownloadEventArgs
            {
                TrackFile = trackFile,
                PercentCompleted = 100,
                Status = TrackStatus.DownloadCompleted
            };
            OnTrackDownloadCompleted(e);
        }

        private void TrackDownloadFail(TrackFile trackFile)
        {
            var e = new TrackDownloadEventArgs
            {
                TrackFile = trackFile,
                Status = TrackStatus.DownloadFailed
            };
            OnTrackDownloadFailed(e);
        }

        private async Task<TrackStatus> DownloadTrackAsync(TrackFile trackFile)
        {
            var e = new TrackDownloadEventArgs
            {
                TrackFile = trackFile,
                PercentCompleted = 0,
                Status = TrackStatus.DownloadingTrack
            };

            TrackStatus result;
            try
            {
                var progress = new Progress<ProgressInfo>(progress =>
                {
                    e.PostUpdate(progress);
                    OnTrackDownloadProgressed(e);
                });
                await service.GetDownloader().DownloadAsync(trackFile, progress);
                result = TrackStatus.DownloadCompleted;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{Service}: Track download failed.", service.Name);
                result = TrackStatus.DownloadFailed;
            }

            return result;
        }

        public void CreateArtworkFile(MediaDownloadItem item)
        {
            var image = ImageCache.Instance.GetImage(item.Media);
            var imageName = image?.FileType.AppendExtension($"{item.Media.CreateDefaultFileName()} (Cover)");

            var fileName = Path.Combine(item.Context.MediaFolder, imageName);
            AthameWriter.Write(fileName, image);
        }

        public void CreateMediaInfo(MediaDownloadItem item)
        {
            if (item.Media.MediaType == MediaType.Track) return;

            try
            {
                var info = CreateMediaInfo(item.Media);
                var fileName = Path.Combine(item.Context.MediaFolder, info.Name);
                AthameWriter.Write(fileName, info);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Exception occurred when writing mediainfo file.");
            }
        }

        public void CreatePlaylistFile(MediaDownloadItem item)
        {
            if (item.Media.MediaType == MediaType.Track) return;

            try
            {
                var playlistFile = CreatePlaylist(Settings.PlaylistFileType)
                    .BuildContent(item.TrackFiles);

                var fileName = Path.Combine(item.Context.MediaFolder, item.Media.CreateDefaultFileName());
                AthameWriter.Write(fileName, playlistFile);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Exception occurred when writing playlist file");
            }
        }

        public static MediaInfo CreateMediaInfo(IMedia media)
            => media.MediaType switch
            {
                MediaType.Album    => new AlbumInfo().BuildContent(media),
                MediaType.Playlist => new PlaylistInfo().BuildContent(media),
                _ => throw new InvalidOperationException()
            };

        public static PlaylistFile CreatePlaylist(PlaylistFileType type)
           => type switch
           {
               PlaylistFileType.M3U => new M3UFile(),
               PlaylistFileType.PLS => new PLSFile(),
               _ => throw new InvalidOperationException()
           };

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

        /// <summary>
        /// Raised when a track download fails.
        /// </summary>
        public event EventHandler<TrackDownloadEventArgs> TrackDownloadFailed;
        protected void OnTrackDownloadFailed(TrackDownloadEventArgs e)
            => TrackDownloadFailed?.Invoke(this, e);

        /// <summary>
        /// Raised when a track download is skipped.
        /// </summary>
        public event EventHandler<TrackDownloadEventArgs> TrackDownloadSkipped;
        protected void OnTrackDownloadSkipped(TrackDownloadEventArgs e)
            => TrackDownloadSkipped?.Invoke(this, e);
        #endregion
    }
}
