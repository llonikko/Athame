using Athame.Plugin.Api.Service;
using System;
using System.Collections.Generic;

namespace Athame.Plugin.Api.Interface
{
    public interface IMedia
    {
        /// <summary>
        /// The media's ID
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// The human-readable title of the media.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// The artist. May be null.
        /// </summary>
        Artist Artist { get; set; }

        /// <summary>
        /// The duration of the entire media.
        /// </summary>
        TimeSpan Duration { get; set; }

        /// <summary>
        /// The media's cover artwork. May be null.
        /// </summary>
        MediaCover Cover { get; set; }

        /// <summary>
        /// A list of custom metadata to associate with the media.
        /// </summary>
        IReadOnlyCollection<Metadata> CustomMetadata { get; set; }

        /// <summary>
        /// The type of media.
        /// </summary>
        MediaType MediaType { get; }
    }
}
