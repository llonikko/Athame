using System.Linq;
using Athame.PluginAPI.Service;

namespace Athame.Core.Utils
{
    public static class MediaCollectionExtensions
    {
        public static int GetAvailableTracksCount(this IMediaCollection collection)
        {
            return collection.Tracks.Sum(track => track.IsDownloadable ? 1 : 0);
        }
    }
}
