using System;
using Athame.PluginAPI.Downloader;
using Athame.PluginAPI.Service;
using CenterCLR;

namespace Athame.Core.Utils
{
    public static class TrackFileExtensions
    {
        public static string GetPath(this TrackFile trackFile, string pathFormat, IMediaCollection collection)
        {
            var cleanedFilePath = trackFile.Track.GetBasicPath(pathFormat, collection);
            return trackFile.FileType.Append(cleanedFilePath);
        }

        public static string GetBasicPath(this Track track, string pathFormat, IMediaCollection collection)
        {
            // Hacky method to clean the file path
            var formatStrComponents = PathHelpers.SplitPathFormat(pathFormat);
            var newFormat = String.Join("\0", formatStrComponents);
            var vars = Dictify.ObjectToDictionary(track);
            vars["PlaylistName"] = collection.Title;
            vars["CollectionName"] = collection.Title;
            vars["ServiceName"] = collection.Title;
            var finalPath = PathHelpers.CleanPath(Named.Format(newFormat, vars));
            return finalPath;
        }
    }
}
