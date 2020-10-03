using System;
using System.Collections.Generic;
using Athame.Core.Download;
using DynamicData;

namespace Athame.Avalonia
{
    public class MediaDownloadSource 
    {
        private readonly SourceCache<MediaDownloadService, string> source 
            = new SourceCache<MediaDownloadService, string>(ms => ms.Media.Id);
        public IObservable<IChangeSet<MediaDownloadService, string>> Connect() 
            => source.Connect();

        public void Add(MediaDownloadService mediaService)
            => source.AddOrUpdate(mediaService);

        public void Remove(MediaDownloadService mediaService)
            => source.Remove(mediaService);
        
        public IEnumerable<MediaDownloadService> Items
            => source.Items;

        public void Clear()
            => source.Clear();

        public int Count
            => source.Count;
    }
}
