using Athame.Core.Interface;
using Athame.Core.Plugin;
using Athame.Core.Settings;
using Athame.Core.Utilities;
using Athame.Plugin.Api.Downloader;
using Athame.Plugin.Api.Service;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Athame.Core.Download
{
    public class MediaDownloadManager
    {
        private readonly MediaCollectionDownloader mediaDownloader;
        private readonly TrackCollectionDownloader trackDownloader;

        public IMediaCollectionDownloader MediaDownloader
            => mediaDownloader;
        public ITrackCollectionDownloader TrackDownloader
            => trackDownloader;

        public AthameSettings Settings { get; set; }

        public MediaDownloadManager()
        {
            trackDownloader = new TrackCollectionDownloader();
            mediaDownloader = new MediaCollectionDownloader(trackDownloader);

            mediaDownloader.MediaDownloadStarted += MediaDownloadStarted;
            mediaDownloader.MediaDownloadCompleted += MediaDownloadCompleted;

            trackDownloader.TrackDownloadStarted += TrackDownloadStarted;
            trackDownloader.TrackDownloadProgressChanged += TrackDownloadProgressChanged;
            trackDownloader.TrackDownloadCompleted += TrackDownloadCompleted;
        }

        public async Task StartDownloadAsync(IEnumerable<MediaItem> items)
        {
            await mediaDownloader.DownloadAsync(items);
        }

        public void StopDownload()
        {
        }

        public virtual void MediaDownloadStarted(object sender, MediaDownloadEventArgs e)
        {
            var descriptor = e.CurrentMediaDownload.Descriptor;
            var preference = Settings.GetPreference(descriptor.MediaType);
            var context = new MediaDownloadContext(preference);

            context.CreateMediaFolder(e.CurrentMediaDownload.Media);

            trackDownloader.Context = context;
            trackDownloader.MediaService = MediaServiceManager.GetService(descriptor.OriginalUri);
        }

        public virtual void MediaDownloadCompleted(object sender, MediaDownloadEventArgs e)
        {
            var media = e.CurrentMediaDownload.Media;

            if (MediaType.Track == media.MediaType) return;

            var path = trackDownloader.Context.MediaFolder;
            var trackFiles = trackDownloader.Context.TrackFiles;

            CreateArtworkFile(path, media);
            CreateMediaInfo(path, media);
            CreatePlaylistFile(path, media, trackFiles, Settings.PlaylistFileType);

            trackDownloader.Context = null;
            trackDownloader.MediaService = null;
        }

        public virtual void TrackDownloadStarted(object sender, TrackDownloadEventArgs e)
        {
        }

        public virtual void TrackDownloadProgressChanged(object sender, TrackDownloadEventArgs e)
        {
        }

        public virtual void TrackDownloadCompleted(object sender, TrackDownloadEventArgs e)
        {
            if (TrackStatus.DownloadCompleted == e.Status)
            {
                TrackTagger.Tag(e.TrackFile, Settings.WriteWatermark);
            }
        }

        public void CreateArtworkFile(string path, IMedia media)
        {
            var image = ImageCache.Instance.GetImage(media);
            var imageName = image?.FileType.AppendExtension($"{PathHelpers.CreateDefaultFileName(media)} (Cover)");

            var fileName = Path.Combine(path, imageName);
            AthameWriter.Write(fileName, image);
        }

        public void CreateMediaInfo(string path, IMediaCollection media)
        {
            try
            {
                var info = GetMediaInfo(media);
                var fileName = Path.Combine(path, info.Name);
                AthameWriter.Write(fileName, info);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Exception occurred when writing mediainfo file.");
            }
        }

        public void CreatePlaylistFile(string path, IMedia media, IEnumerable<TrackFile> trackFiles, PlaylistFileType type)
        {
            try
            {
                var playlistFile = CreatePlaylist(type)
                    .BuildContent(trackFiles);

                var fileName = Path.Combine(path, PathHelpers.CreateDefaultFileName(media));
                AthameWriter.Write(fileName, playlistFile);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Exception occurred when writing playlist file");
            }
        }

        public static PlaylistFile CreatePlaylist(PlaylistFileType type)
           => type switch
           {
               PlaylistFileType.M3U => new M3UFile(),
               PlaylistFileType.PLS => new PLSFile(),
               _ => throw new InvalidOperationException()
           };

        public static MediaInfo GetMediaInfo(IMediaCollection media)
            => media.MediaType switch
            {
                MediaType.Album    => new AlbumInfo().BuildContent(media),
                MediaType.Playlist => new PlaylistInfo().BuildContent(media),
                _ => throw new InvalidOperationException()
            };
    }
}
