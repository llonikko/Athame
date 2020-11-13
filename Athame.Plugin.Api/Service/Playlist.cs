using Athame.Plugin.Api.Interface;
using System;
using System.Collections.Generic;

namespace Athame.Plugin.Api.Service
{
    /// <summary>
    /// Represents an ordered, arbitrary list of tracks.
    /// </summary>
    public class Playlist : ITrackCollection
    {
        /// <summary>
        /// The service-specific identifier of the playlist.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The title of the playlist.
        /// </summary>
        public string Title { get; set; }

        public Artist Artist { get; set; } 
            = new Artist { Name = "Various Artists" };

        /// <summary>
        /// The description of the playlist.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The tracks contained within the playlist.
        /// </summary>
        public List<Track> Tracks { get; set; }

        /// <summary>
        /// The duration of the playlist.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// The date of when the playlist was initially published. May be null.
        /// </summary>
        public DateTime? PublishDate { get; set; }

        /// <summary>
        /// The date of when the playlist was last modified. May be null.
        /// </summary>
        public DateTime? LastModifiedDate { get; set; }

        /// <summary>
        /// A picture representing the playlist. May be null.
        /// </summary>
        public MediaCover Cover { get; set; }

        public IReadOnlyCollection<Metadata> CustomMetadata { get; set; }

        public MediaType MediaType => MediaType.Playlist;
    }
}
