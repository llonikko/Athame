using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Athame.PluginAPI.Service;

namespace Athame.Core.DownloadAndTag
{
    public class ImageCache
    {
        public static readonly ImageCache Instance = new ImageCache();

        private readonly Dictionary<string, ImageCacheEntry> items = new Dictionary<string, ImageCacheEntry>();

        public async Task AddByDownload(string smid, Picture picture)
        {
            var data = await picture.GetLargestVersionAsync();

            items.Add(smid, new ImageCacheEntry
            {
                Data = data,
                Picture = picture
            });
        }

        public void AddNull(string smid)
        {
            items[smid] = null;
        }

        private const string DefaultKey = "_default";
        public ImageCacheEntry GetDefault()
        {
            return items.ContainsKey(DefaultKey) ? items[DefaultKey] : null;
        }

        public void SetDefault(ImageCacheEntry file)
        {
            items[DefaultKey] = file;
        }

        public bool HasItem(string url)
        {
            return items.ContainsKey(url);
        }

        public ImageCacheEntry Get(string url)
        {
            return items[url];
        }

        public void WriteToFile(string smid, string filePath)
        {
            var item = items[smid];
            File.WriteAllBytes(item.Picture.FileType.Append(filePath), item.Data);
        }
    }
}
