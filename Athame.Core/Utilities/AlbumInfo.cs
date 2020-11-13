using Athame.Core.Extensions;
using Athame.Plugin.Api.Interface;
using Athame.Plugin.Api.Service;
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
                
                BuildTrackListTextInfo(content, album.GetTracks(disc));
                content.AppendLine();
            }
        }
    }
}
