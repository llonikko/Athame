using Athame.Plugin.Api.Downloader;
using Athame.Plugin.Api.Service;
using System.Collections.Generic;

namespace Athame.Core.Download
{
    public class MediaDownloadItem
    {
        public ICollection<TrackFile> TrackFiles { get; }
        public IMediaCollection Media { get; }
        public MediaDownloadContext Context { get; }
        public MediaDescriptor Descriptor { get; set; }

        public MediaDownloadItem(MediaDescriptor descriptor, MediaDownloadContext context, IMediaCollection media)
        {
            Descriptor = descriptor;
            Context = context;
            Media = media;
            TrackFiles = new List<TrackFile>();
        }

        public void CreateMediaFolder()
            => Context.CreateMediaFolder(Media);

        public void CreatePath(TrackFile trackFile)
            => trackFile.FullPath = Context.CreateFullPath(trackFile);
    }
}
