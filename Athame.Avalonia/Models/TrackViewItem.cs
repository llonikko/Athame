using Athame.Avalonia.Resources;
using Athame.Plugin.Api.Service;
using Avalonia.Media.Imaging;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Athame.Avalonia.Models
{
    public class TrackViewItem : ReactiveObject
    {
        public string DiscTrackNumber
            => $"{Track.DiscNumber}/{Track.TrackNumber}";

        public Track Track { get; set; }
        public bool IsDownloaded { get; set; }
        [Reactive]
        public Bitmap ImageStatus { get; set; }

        public IEnumerable<Metadata> Flags
            => Track.CustomMetadata.Where(m => m.CanDisplay && m.IsFlag);

        public TrackViewItem(Track track)
        {
            Track = track;
            ImageStatus = track.IsDownloadable ? Images.Info : Images.Error;
        }

        public static IEnumerable<TrackViewItem> Create(IEnumerable<Track> tracks)
        {
            var v = new List<TrackViewItem>();
            foreach (var t in tracks)
            {
                v.Add(new TrackViewItem(t));
            }
            return v;
        }
    }
}
