using Athame.Plugin.Api.Service;
using System.Collections.Generic;

namespace Athame.Core.Download
{
    public class ImageCache
    {
        private readonly Dictionary<string, MediaCover> imageCache = new Dictionary<string, MediaCover>();

        public static ImageCache Instance { get; } = new ImageCache();

        private ImageCache() 
        {
        }

        public void AddEntry(MediaCover image)
            => AddEntry(image.Id, image);

        public void AddEntry(string key, MediaCover image)
            => imageCache.Add(key, image);

        public void AddNull(IMedia media)
            => AddNull(media.Cover?.Id);

        public void AddNull(string key)
            => imageCache[key] = null;

        public bool HasImage(IMedia media)
            => HasItem(media.Cover?.Id);

        public bool HasItem(string key)
            => (key != null) && imageCache.ContainsKey(key);

        public MediaCover GetImage(string key)
            => imageCache[key];

        public MediaCover GetImage(IMedia media)
        {
            var id = media.Cover.Id;
            return HasItem(id) ? GetImage(id) : null;
        }
    }
}
