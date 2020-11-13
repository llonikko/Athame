using Athame.Plugin.Api.Interface;
using System;
using System.Collections.Generic;

namespace Athame.Plugin.Api.Service
{
    /// <summary>
    /// Represents a single audio track.
    /// </summary>
    public class Track : IMedia
    {
        /// <summary>
        /// The service-specific identifier of the track.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The track's title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The name of the artist.
        /// </summary>
        public Artist Artist { get; set; }

        /// <summary>
        /// The album this specific track is featured on.
        /// </summary>
        public Album Album { get; set; }

        /// <summary>
        /// The playlist this specific track is featured on.
        /// </summary>
        public Playlist Playlist { get; set; }

        /// <summary>
        /// The index of the track relative to the disc it's on in the album. Must be > 0.
        /// </summary>
        public int TrackNumber { get; set; }

        /// <summary>
        /// The duration of the track.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// The disc (or volume) in the album the track is on. Must be > 0.
        /// </summary>
        public int DiscNumber { get; set; }

        /// <summary>
        /// The track's genre.
        /// </summary>
        public string Genre { get; set; }

        /// <summary>
        /// The track's composer.
        /// </summary>
        public string[] Composer { get; set; }

        /// <summary>
        /// The year the track was released. Must be four digits.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// ISRC 
        /// </summary>
        public string Isrc { get; set; }

        /// <summary>
        /// Copyright
        /// </summary>
        public string Copyright { get; set; }

        /// <summary>
        /// The replay gain of a track.
        /// </summary>
        public double? ReplayGain { get; set; }

        /// <summary>
        /// The peak gain of a track.
        /// </summary>
        public double? Peak { get; set; }

        /// <summary>
        /// If the Artist property of the <see cref="Album"/> property is null, returns the track's <see cref="Artist"/> artist property,
        /// otherwise returning the <see cref="Album"/>'s Artist property. 
        /// </summary>
        public Artist AlbumArtistOrArtist 
            => Album?.Artist ?? Artist;

        /// <summary>
        /// If the track can be downloaded or streamed.
        /// </summary>
        public bool IsDownloadable { get; set; }

        /// <summary>
        /// The track's cover artwork. May be null.
        /// </summary>
        public MediaCover Cover { get; set; }

        /// <summary>
        /// A list of custom metadata to associate with the track.
        /// </summary>
        public IReadOnlyCollection<Metadata> CustomMetadata { get; set; }

        public MediaType MediaType => MediaType.Track;

        /// <summary>
        /// Returns a new <see cref="SingleTrackCollection"/> for this track.
        /// </summary>
        /// <returns>A new <see cref="SingleTrackCollection"/> containing only this track.</returns>
        public SingleTrackCollection AsCollection() 
            => new SingleTrackCollection(this);

        public int TotalSeconds
        {
            get
            {
                var seconds = (Duration != null ? Duration.TotalSeconds : 0);
                return (int)Math.Round((double)(seconds));
            }
        }
    }
}
