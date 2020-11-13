using Athame.Plugin.Api.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Athame.Plugin.Api.Service
{
    /// <summary>
    /// Represents a single track for use in scenarios where a <see cref="IMediaCollection"/> is required.
    /// This does not need to be handled by your service implementation, and is meant for internal use only.
    /// </summary>
    public class SingleTrackCollection : ITrackCollection
    {
        /// <summary>
        /// Constructs a new instance based on a single track. Also sets properties of <see cref="IMediaCollection"/>
        /// to this track's equivalent properties.
        /// </summary>
        /// <param name="track">The single track to use.</param>
        public SingleTrackCollection(Track track)
        {
            Id = track.Id;
            Title = track.Title;
            Artist = track.Artist;
            Tracks = new List<Track>(1) { track };
            Duration = track.Duration;
            Cover = track.Album.Cover;
            CustomMetadata = track.CustomMetadata;
        }

        public string Id { get; set; }

        public string Title { get; set; }

        public Artist Artist { get; set; }

        public List<Track> Tracks { get; set; }

        public TimeSpan Duration { get; set; }

        /// <summary>
        /// The single track album's cover artwork. May be null.
        /// </summary>
        public MediaCover Cover { get; set; }

        public IReadOnlyCollection<Metadata> CustomMetadata { get; set; }

        public MediaType MediaType => MediaType.Track;

        /// <summary>
        /// The only track in this collection.
        /// </summary>
        public Track Track => Tracks.First();
    }
}
