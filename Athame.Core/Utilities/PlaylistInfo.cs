using Athame.Core.Extensions;
using Athame.Plugin.Api.Interface;
using Athame.Plugin.Api.Service;
using System.Text;

namespace Athame.Core.Utilities
{
    public class PlaylistInfo : MediaInfo
    {
        public override string Name => "PlaylistInfo";

        protected override void BuildInfo(StringBuilder content, IMedia media)
        {
            Playlist playlist = media as Playlist;

            content.AppendLine($"{playlist.Title}");
            content.AppendLine($"{playlist.Description}");
            content.AppendLine($"{playlist.Tracks.Count} Tracks - {playlist.FormattedDuration()}");
            content.AppendLine();

            foreach (var track in playlist.Tracks)
            {
                content.Append($"{track.Title}");
                content.AppendLine();
            }
        }
    }
}
