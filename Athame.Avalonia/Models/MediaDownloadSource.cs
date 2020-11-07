using System;
using System.Collections.Generic;
using Athame.Core.Download;
using DynamicData;

namespace Athame.Avalonia
{
    public class MediaDownloadSource 
    {
        private readonly SourceCache<MediaItem, string> source 
            = new SourceCache<MediaItem, string>(ms => ms.Media.Id);
        public IObservable<IChangeSet<MediaItem, string>> Connect() 
            => source.Connect();

        public void Add(MediaItem mediaService)
            => source.AddOrUpdate(mediaService);

        public void Remove(MediaItem mediaService)
            => source.Remove(mediaService);
        
        public IEnumerable<MediaItem> Items
            => source.Items;

        public void Clear()
            => source.Clear();

        public int Count
            => source.Count;
    }
}
