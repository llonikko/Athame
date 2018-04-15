using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Athame.Core.DownloadAndTag;
using Athame.PluginAPI.Downloader;
using Athame.PluginAPI.Service;

namespace Athame.Core.Utils
{
    /// <summary>
    /// Writes an <see cref="IMediaCollection"/> to a playlist file contained in the collection's destination directory.
    /// </summary>
    public class PlaylistWriter
    {
        /// <summary>
        /// The collection being referenced
        /// </summary>
        public EnqueuedCollection Collection { get; set; }

        /// <summary>
        /// The trackfiles of the collection
        /// </summary>
        public List<TrackFile> TrackFiles { get; set; }

        /// <summary>
        /// Initialises a new instance of <see cref="PlaylistWriter"/>.
        /// </summary>
        /// <param name="collection">The collection to use</param>
        /// <param name="files">The files to include in the playlist</param>
        public PlaylistWriter(EnqueuedCollection collection, List<TrackFile> files)
        {
            Collection = collection;
            TrackFiles = files;
        }

        /// <summary>
        /// Gets a track's duration in seconds, returning 0 if it is null.
        /// </summary>
        /// <param name="track">The track to get the duration from</param>
        /// <returns>The duration in seconds, or 0 if null.</returns>
        private int GetTrackDurationSeconds(TrackFile track)
        {
            return (int)Math.Round(track.Track.Duration?.TotalSeconds ?? 0);
        }

        /// <summary>
        /// Writes the contents of the specified StringBuilder to the collection's destination as a
        /// playlist named after the collection title, with the specified extension.
        /// </summary>
        /// <param name="extension">The extension of the file.</param>
        /// <param name="sb">The StringBuilder containing the playlist file contents.</param>
        private void WriteFile(string extension, StringBuilder sb)
        {
            var path = Path.Combine(Collection.Destination, Collection.MediaCollection.Title + "." + extension);
            File.WriteAllText(path, sb.ToString());
        }

        /// <summary>
        /// Writes a Unicode M3U format playlist to disk.
        /// </summary>
        public void WriteM3U8()
        {
            var sb = new StringBuilder("#EXTM3U");
            sb.AppendLine();
            sb.AppendLine();
            foreach (var trackFile in TrackFiles)
            {
                var seconds = GetTrackDurationSeconds(trackFile);
                sb.AppendLine($"#EXTINF:{seconds},{trackFile.Track.Artist} - {trackFile.Track.Title}");
                sb.Append(".");
                sb.Append(Path.DirectorySeparatorChar);
                sb.AppendLine(Collection.GetRelativePath(trackFile));
                sb.AppendLine();
            }
            WriteFile("m3u8", sb);
        }

        /// <summary>
        /// Writes a PLS format playlist to disk.
        /// </summary>
        public void WritePLS()
        {
            var sb = new StringBuilder("[playlist]");
            sb.AppendLine();
            sb.AppendLine();
            var i = 0;
            foreach (var trackFile in TrackFiles)
            {
                i++;
                sb.AppendLine($"File{i}=.{Path.DirectorySeparatorChar}{Collection.GetRelativePath(trackFile)}");
                var duration = GetTrackDurationSeconds(trackFile);
                if (duration > 0)
                {
                    sb.AppendLine($"Length{i}={duration}");
                }
                sb.AppendLine($"Title{i}={trackFile.Track.Title}");
                sb.AppendLine();
            }
            sb.AppendLine($"NumberOfEntries={i}");
            sb.AppendLine("Version=2");
            WriteFile("pls", sb);
        }
    }
}
