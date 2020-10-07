using Athame.Core.Extensions;
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
    public class MediaDownloadService
    {
        public ICollection<TrackFile> TrackFiles { get; }

        public IMediaService MediaService { get; }
        public IMediaCollection Media { get; }
        public MediaDownloadContext Context { get; }

        public MediaDownloadService(IMediaService service, MediaDownloadContext context, IMediaCollection media)
        {
            MediaService = service;
            Context = context;
            Media = media;
            TrackFiles = new List<TrackFile>();
        }

        public void CreateMediaFolder()
            => Context.CreateMediaFolder(Media);

        public async Task<TrackFile> CreateTrackFile(Track track)
            => await MediaService
                .GetDownloadableTrackAsync(track)
                .ConfigureAwait(false);

        public void CreatePath(TrackFile trackFile)
            => trackFile.FullPath = Context.CreateFullPath(trackFile);

        public async Task DownloadArtworkAsync(IMedia media)
        {
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

        public void CreateArtworkFile(ArtworkFileName name)
        {
            var image = ImageCache.Instance.GetImage(Media);
            var imageName = image?.FileType.AppendExtension(name == ArtworkFileName.AsCover 
                ? "Cover" 
                : Media.CreateDefaultFileName());

            var fileName = Path.Combine(Context.MediaFolderPath, imageName);
            AthameWriter.Write(fileName, image);
        }

        public void CreateMediaInfo()
        {
            if (Media.MediaType == MediaType.Track) return;

            try
            {
                var info = MediaInfo
                    .Create(Media.MediaType)
                    .BuildContent(Media);

                var fileName = Path.Combine(Context.MediaFolderPath, info.Name);
                AthameWriter.Write(fileName, info);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "{Service}: Exception occurred when writing mediainfo file.", MediaService.Name);
            }
        }

        public void CreatePlaylistFile(PlaylistFileType type)
        {
            if (Media.MediaType == MediaType.Track || type == PlaylistFileType.None) return;

            try
            {
                var playlistFile = PlaylistFile
                    .Create(type)
                    .BuildContent(TrackFiles);

                var fileName = Path.Combine(Context.MediaFolderPath, Media.CreateDefaultFileName());
                AthameWriter.Write(fileName, playlistFile);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "{Service}: Exception occurred when writing playlist file", MediaService.Name);
            }
        }
    }
}
