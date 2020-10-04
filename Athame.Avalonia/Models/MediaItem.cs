using Athame.Core.Download;
using Athame.Plugin.Api.Service;
using System.Collections.Generic;
using System.Linq;

namespace Athame.Avalonia.Models
{
    public class MediaItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<TrackItem> Items { get; }
        public IEnumerable<Metadata> Flags { get; }

        public MediaItem(MediaDownloadService service)
        {
            Id = service.Media.Id;
            Name = $"{service.Media.Title} - {service.Media.Artist}";
            
            Items = new List<TrackItem>();
            foreach(var track in service.Media.Tracks)
            {
                Items.Add(new TrackItem 
                {
                    Track = track,
                    IsDownloaded = false
                });
            }

            Flags = service.Media.CustomMetadata?.Where(m => m.CanDisplay && m.IsFlag);
        }
    }

    public class TrackItem
    {
        public string DiscTrackNumber 
            => $"{Track.DiscNumber}/{Track.TrackNumber}";

        public Track Track { get; set; }
        public bool IsDownloaded { get; set; }

        public IEnumerable<Metadata> Flags
            => Track.CustomMetadata.Where(m => m.CanDisplay && m.IsFlag);
    }
}
