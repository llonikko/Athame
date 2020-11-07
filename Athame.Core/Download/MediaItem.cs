using Athame.Plugin.Api.Service;

namespace Athame.Core.Download
{
    public class MediaItem
    {
        public IMediaCollection Media { get; }
        public MediaDescriptor Descriptor { get; set; }

        public MediaItem(IMediaCollection media, MediaDescriptor descriptor)
        {
            Descriptor = descriptor;
            Media = media;
        }
    }
}
