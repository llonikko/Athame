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
        public IEnumerable<TrackItem> TrackItems { get; }
        public IEnumerable<Metadata> Flags { get; }

        public MediaItem(MediaDownloadService service)
        {
            Id = service.Media.Id;
            Name = $"{service.Media.Title} - {service.Media.Artist}";
            TrackItems = CreateTrackItems(service.Media.Tracks);
            Flags = service.Media.CustomMetadata?.Where(m => m.CanDisplay && m.IsFlag);
        }

        private IEnumerable<TrackItem> CreateTrackItems(IEnumerable<Track> tracks)
        {
            var v = new List<TrackItem>();
            foreach (var t in tracks)
            {
                v.Add(new TrackItem
                {
                    Track = t,
                    IsDownloaded = false
                });
            }
            return v;
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
