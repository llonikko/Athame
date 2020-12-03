using Athame.Core.Extensions;
using Athame.Plugin.Api.Interface;
using Athame.Plugin.Api.Service;
using System.Linq;
using System.Text;

namespace Athame.Core.Utilities
{
    public class AlbumInfo : MediaInfo
    {
        public override string Name => "AlbumInfo";

        protected override void BuildInfo(StringBuilder content, IMedia media)
        {
            Album album = media as Album;

            content.AppendLine($"{album.Title}");
            content.AppendLine($"by {album.Artist} - {album.NumberOfTracks} Tracks - {album.FormattedDuration()}");
            content.AppendLine($"Released {album.Year}");
            content.AppendLine();

            for (var disc = 1; disc <= album.NumberOfDiscs; disc++)
            {
                if (album.NumberOfDiscs > 1) 
                {
                    content.AppendLine($"CD{disc}");
                }

                var tracks = album.GetTracks(disc);
                string format = tracks.Count() > 99 ? "{0:000}" : "{0:00}";
                foreach (var track in tracks)
                {
                    content.Append(string.Format(format, track.TrackNumber));
                    content.Append(" - ");
                    content.Append($"{track.Title}");
                    content.AppendLine();
                }

                content.AppendLine();
            }
        }
    }
}
