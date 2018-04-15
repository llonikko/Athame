using System;
using System.IO;
using Athame.Core.Utils;
using Athame.PluginAPI.Downloader;
using Athame.PluginAPI.Service;
using TagLib;
using File = TagLib.File;
using SysFile = System.IO.File;

namespace Athame.Core.DownloadAndTag
{
    public class TrackTagger
    {
        private const string CopyrightText = "Respect the artists! Pay for music when you can! Downloaded with Athame";

        private static void WriteArtworkFile(string directory, AlbumArtworkSaveFormat saveFormat, Track track, ImageCacheEntry albumArtwork)
        {
            string fileName = null;
            switch (saveFormat)
            {
                case AlbumArtworkSaveFormat.DontSave:
                    break;
                case AlbumArtworkSaveFormat.AsCover:
                    fileName = albumArtwork?.Picture.FileType.Append("cover");
                    break;
                case AlbumArtworkSaveFormat.AsArtistAlbum:
                    fileName = albumArtwork?.Picture.FileType.Append($"{track.Artist} - {track.Album.Title}");
                    if (fileName != null)
                    {
                        fileName = PathHelpers.CleanFilename(fileName);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (fileName == null) return;
            SysFile.WriteAllBytes(Path.Combine(directory, fileName), albumArtwork.Data);
        }

        public static void Write(string serviceName, Track completedTrack, TrackFile trackFile, AlbumArtworkSaveFormat saveFormat, string path)
        {
            // Get album artwork from cache
            ImageCacheEntry albumArtwork = null;
            var smid = completedTrack.Album.GetSmid(serviceName).ToString();
            if (ImageCache.Instance.HasItem(smid))
            {
                albumArtwork = ImageCache.Instance.Get(smid);
            }

            // Write track tags
            var track = completedTrack;
            using (var file = File.Create(new File.LocalFileAbstraction(path),
                trackFile.FileType.MimeType, ReadStyle.Average))
            {
                file.Tag.Title = track.Title;
                file.Tag.Performers = new[] { track.Artist.Name };
                if (track.Album.Artist != null)
                {
                    file.Tag.AlbumArtists = new[] { track.Album.Artist.Name };
                }
                if (track.Genre != null)
                {
                    file.Tag.Genres = new[] {track.Genre};
                }
                file.Tag.Album = track.Album.Title;
                file.Tag.Track = (uint)track.TrackNumber;
                file.Tag.TrackCount = (uint)(track.Album.GetNumberOfTracksOnDisc(track.DiscNumber) ?? 0);
                file.Tag.Disc = (uint)track.DiscNumber;
                file.Tag.DiscCount = (uint)(track.Album.GetTotalDiscs() ?? 0);
                file.Tag.Year = (uint)track.Year;
                file.Tag.Copyright = CopyrightText;
                file.Tag.Comment = CopyrightText;
                if (albumArtwork != null)
                {
                    file.Tag.Pictures = new IPicture[] { new TagLib.Picture(new ByteVector(albumArtwork.Data)) };
                }

                file.Save();
            }

            // Write album artwork to file if requested
            if (albumArtwork == null) return;
            string parentDirectory;
            if (saveFormat != AlbumArtworkSaveFormat.DontSave &&
                (parentDirectory = Path.GetDirectoryName(path)) != null)
            {
                WriteArtworkFile(parentDirectory, saveFormat, track, albumArtwork);
            }
        }
    }
}
