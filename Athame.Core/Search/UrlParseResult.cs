namespace Athame.Core.Search
{
    /// <summary>
    /// The result status of the URL parser after attempting to parse a URL.
    /// </summary>
    public enum UrlParseResult
    {
        /// <summary>
        /// The input was null or whitespace.
        /// </summary>
        NullOrEmptyString,

        /// <summary>
        /// The input was not a valid URL.
        /// </summary>
        InvalidUrl,

        /// <summary>
        /// The URL was valid, but no service corresponds to it.
        /// </summary>
        NoServiceFound,

        /// <summary>
        /// A service was found that matches the URL's host, but it is not authenticated.
        /// At this state and the ones that follow it, the <see cref="UrlResolver.Service"/> property
        /// is the matching service.
        /// </summary>
        ServiceNotAuthenticated,

        /// <summary>
        /// A service was found, but needs to be restored.
        /// </summary>
        ServiceNotRestored,

        /// <summary>
        /// The service does not recognise this URL as pointing to any downloadable media.
        /// </summary>
        NoMedia,

        /// <summary>
        /// The URL points to a downloadable media collection.
        /// </summary>
        Success,

        /// <summary>
        /// An exception occurred at some point. See <see cref="UrlResolver.Exception"/>.
        /// </summary>
        Exception
    }
}
