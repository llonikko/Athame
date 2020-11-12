using Athame.Plugin.Api.Service;
using System;
using System.Collections.Generic;

namespace Athame.Plugin.Api.Interface
{
    /// <summary>
    /// Represents a generic enumerable collection of tracks.
    /// </summary>
    public interface ITrackCollection : IMedia
    {
        /// <summary>
        /// The tracks this collection contains.
        /// </summary>
        List<Track> Tracks { get; set; }
    }
}
