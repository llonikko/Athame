namespace Athame.Core.Download
{
    /// <summary>
    /// Defines what to skip to next when an exception is encountered.
    /// </summary>
    public enum DownloadSkip
    {
        /// <summary>
        /// The downloader should advance to the next item.
        /// </summary>
        NextItem,

        /// <summary>
        /// The downloader should advance to the next collection.
        /// </summary>
        NextCollection,

        /// <summary>
        /// The downloader should stop and return immediately.
        /// </summary>
        Fail
    }
}
