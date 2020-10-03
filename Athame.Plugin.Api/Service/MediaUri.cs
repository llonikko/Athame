using System;

namespace Athame.Plugin.Api.Service
{
    /// <summary>
    /// Represents the result of a URL parsed with <see cref="IMediaService.ParseUrl"/>.
    /// </summary>
    public class MediaUri
    {
        /// <summary>
        /// The original <see cref="Uri"/> that was parsed.
        /// </summary>
        public Uri OriginalUri { get; set; }

        /// <summary>
        /// The media type this URL refers to.
        /// </summary>
        public MediaType MediaType { get; set; }

        /// <summary>
        /// The media ID this URL refers to. If <see cref="MediaType"/> is <see cref="MediaType.Unknown"/>,
        /// this should be null to indicate the URL did not point to a valid resource.
        /// </summary>
        public string MediaId { get; set; }
    }
}
