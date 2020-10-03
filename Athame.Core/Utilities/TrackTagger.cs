using Athame.Core.Download;
using Athame.Plugin.Api.Downloader;
using Athame.Plugin.Api.Service;
using System.Reflection;
using TagLib;

namespace Athame.Core.Utilities
{
    public static class TrackTagger
    {
        public static string WatermarkText 
            => $"Downloaded with Athame {Assembly.GetEntryAssembly().GetName().Version}";

        public static void Tag(TrackFile trackFile, bool canWriteWatermark = false)
        {
            var track = trackFile.Track;

            // Get album artwork from cache
            MediaCover coverArt = ImageCache.Instance.GetImage(track.Album);

            // Write track tags
            using var tfile = File.Create(
                new File.LocalFileAbstraction(trackFile.FullPath), trackFile.FileType.MimeType, ReadStyle.Average);

            tfile.Tag.Title = track.Title;
            tfile.Tag.Album = track.Album.Title;
            tfile.Tag.Performers = new[] { track.Artist.Name };

            if (track.Album.Artist != null)
            {
                tfile.Tag.AlbumArtists = new[] { track.Album.Artist.Name };
            }

            tfile.Tag.Genres = new[] { track.Genre };
            tfile.Tag.Track = (uint)track.TrackNumber;

            if (track.Album.Tracks != null)
            {
                tfile.Tag.TrackCount = (uint)(track.Album.GetNumberOfTracks(track.DiscNumber));
            }

            tfile.Tag.Disc = (uint)track.DiscNumber;
            tfile.Tag.DiscCount = (uint)track.Album.NumberOfDiscs;
            tfile.Tag.Year = (uint)track.Year;
            tfile.Tag.Composers = track.Composer;
            tfile.Tag.ISRC = track.Isrc;
            tfile.Tag.Copyright = track.Copyright;

            if (track.ReplayGain.HasValue)
            {
                tfile.Tag.ReplayGainTrackGain = track.ReplayGain.Value;
            }

            if (track.Peak.HasValue)
            {
                tfile.Tag.ReplayGainTrackPeak = track.Peak.Value;
            }

            if (canWriteWatermark)
            {
                tfile.Tag.Comment = WatermarkText;
            }

            if (coverArt != null)
            {
                IPicture pic = new Picture(new ByteVector(coverArt.Data));
                tfile.Tag.Pictures = new IPicture[] 
                {
                    new Picture
                    {
                        Data = pic.Data.Data,
                        MimeType = pic.MimeType
                    }
                };
            }

            tfile.Save();
        }
    }
}
