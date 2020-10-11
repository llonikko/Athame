using System;
using System.Collections.Generic;

namespace Athame.Plugin.Api.Service
{
    /// <summary>
    /// Represents a generic enumerable collection of tracks.
    /// </summary>
    public interface IMediaCollection : IMedia
    {
        /// <summary>
        /// The tracks this collection contains.
        /// </summary>
        List<Track> Tracks { get; set; }
    }
}
