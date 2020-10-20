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

        public MediaItem(IMediaCollection media)
        {
            Id = media.Id;
            Name = $"{media.Title} - {media.Artist}";
            TrackItems = TrackItem.Create(media.Tracks);
            Flags = media.CustomMetadata?.Where(m => m.CanDisplay && m.IsFlag);
        }

        public TrackItem GetTrackItem(Track track)
            => TrackItems.First(x => x.Track.Id == track.Id);

        public bool IsDownloadable()
            => TrackItems.Any(x => x.Track.IsDownloadable);
    }
}
