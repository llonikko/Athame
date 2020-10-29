using System;
using System.Collections.Generic;
using Athame.Core.Download;
using DynamicData;

namespace Athame.Avalonia
{
    public class MediaDownloadSource 
    {
        private readonly SourceCache<MediaDownloadItem, string> source 
            = new SourceCache<MediaDownloadItem, string>(ms => ms.Media.Id);
        public IObservable<IChangeSet<MediaDownloadItem, string>> Connect() 
            => source.Connect();

        public void Add(MediaDownloadItem mediaService)
            => source.AddOrUpdate(mediaService);

        public void Remove(MediaDownloadItem mediaService)
            => source.Remove(mediaService);
        
        public IEnumerable<MediaDownloadItem> Items
            => source.Items;

        public void Clear()
            => source.Clear();

        public int Count
            => source.Count;
    }
}
