namespace Athame.Core.Search
{
    /// <summary>
    /// The result status of the URL parser after attempting to parse a URL.
    /// </summary>
    public enum UrlParseStatus
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
        /// The service does not recognise this URL as pointing to any downloadable media.
        /// </summary>
        NoMedia,

        /// <summary>
        /// The URL points to a downloadable media collection.
        /// </summary>
        Success
    }
}
