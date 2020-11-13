using Athame.Plugin.Api.Interface;
using Athame.Plugin.Api.Service;

namespace Athame.Core.Download
{
    public class MediaItem
    {
        public ITrackCollection Media { get; }
        public MediaDescriptor Descriptor { get; set; }

        public MediaItem(ITrackCollection media, MediaDescriptor descriptor)
        {
            Descriptor = descriptor;
            Media = media;
        }
    }
}
