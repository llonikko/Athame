using Athame.Core.Utilities;
using Athame.Plugin.Api.Interface;
using Athame.Plugin.Api.Service;
using System.Collections.Generic;
using System.Linq;

namespace Athame.Core.Extensions
{
    public static class MediaCollectionExtensions
    {
        public static int GetDownloadableTracksCount(this ITrackCollection media)
            => media.Tracks.Sum(track => track.IsDownloadable ? 1 : 0);

        public static IEnumerable<Track> GetDownloadableTracks(this ITrackCollection media)
            => from t in media.Tracks where t.IsDownloadable select t;

        public static bool IsDownloadable(this ITrackCollection media)
            => media.Tracks.Any(t => t.IsDownloadable);

        public static string CreateDefaultFileName(this IMedia media)
            => PathHelpers.CleanFilename($"{media.Artist} - {media.Title}");

        public static string FormattedDuration(this IMedia media) 
            => (media.Duration.Hours < 1)
                ? media.Duration.ToString(@"%m\:ss")
                : media.Duration.ToString(@"%h\:mm\:ss");
    }
}
