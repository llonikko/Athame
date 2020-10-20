using Athame.Avalonia.Resources;
using Athame.Plugin.Api.Service;
using Avalonia.Media.Imaging;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Athame.Avalonia.Models
{
    public class TrackItem : ReactiveObject
    {
        public string DiscTrackNumber
            => $"{Track.DiscNumber}/{Track.TrackNumber}";

        public Track Track { get; set; }
        public bool IsDownloaded { get; set; }
        [Reactive]
        public Bitmap ImageStatus { get; set; }

        public IEnumerable<Metadata> Flags
            => Track.CustomMetadata.Where(m => m.CanDisplay && m.IsFlag);

        public TrackItem(Track track)
        {
            Track = track;
            ImageStatus = track.IsDownloadable ? Images.Info : Images.Error;
        }

        public static IEnumerable<TrackItem> Create(IEnumerable<Track> tracks)
        {
            var v = new List<TrackItem>();
            foreach (var t in tracks)
            {
                v.Add(new TrackItem(t));
            }
            return v;
        }
    }
}
