using Athame.Plugin.Api.Interface;
using Athame.Plugin.Api.Service;
using System.Collections.Generic;
using System.Linq;

namespace Athame.Avalonia.Models
{
    public class MediaViewItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<TrackViewItem> TrackViewItems { get; }
        public IEnumerable<Metadata> Flags { get; }

        public MediaViewItem(ITrackCollection media)
        {
            Id = media.Id;
            Name = $"{media.Title} - {media.Artist}";
            TrackViewItems = TrackViewItem.Create(media.Tracks);
            Flags = media.CustomMetadata?.Where(m => m.CanDisplay && m.IsFlag);
        }

        public TrackViewItem GetTrackViewItem(Track track)
            => TrackViewItems.SingleOrDefault(x => x.Track.Id == track.Id);

        public bool IsDownloadable()
            => TrackViewItems.Any(x => x.Track.IsDownloadable);
    }
}
