using Athame.Plugin.Api.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Athame.Plugin.Api.Service
{
    public class Album : ITrackCollection
    {
        /// <summary>
        /// Service-specific identifier for the album. Not null.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The album's title. Not null.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The album artist. May be null.
        /// </summary>
        public Artist Artist { get; set; }

        /// <summary>
        /// The album's tracks. May be null.
        /// </summary>
        public List<Track> Tracks { get; set; }

        /// <summary>
        /// The duration of the album.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// The album's total number tracks
        /// </summary>
        public int NumberOfTracks { get; set; }

        /// <summary>
        /// The album's total number discs
        /// </summary>
        public int NumberOfDiscs { get; set; }

        /// <summary>
        /// The album's cover artwork. May be null.
        /// </summary>
        public MediaCover Cover { get; set; }

        /// <summary>
        /// The album's type, if the service supports it. Defaults to <see cref="AlbumType.Album"/>.
        /// </summary>
        public AlbumType Type { get; set; }

        /// <summary>
        /// The year the album was released.
        /// </summary>
        public int Year { get; set; }

        public IReadOnlyCollection<Metadata> CustomMetadata { get; set; }

        public MediaType MediaType => MediaType.Album;

        public int GetNumberOfTracks(int disc)
            => (NumberOfDiscs == 1) ? NumberOfTracks : GetTracks(disc).Count();

        public IEnumerable<Track> GetTracks(int disc)
            => from t in Tracks where t.DiscNumber == disc 
               select t;
    }
}
