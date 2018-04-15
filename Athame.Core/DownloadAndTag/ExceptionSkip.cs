namespace Athame.Core.DownloadAndTag
{
    /// <summary>
    /// Defines what to skip to next when an exception is encountered.
    /// </summary>
    public enum ExceptionSkip
    {
        /// <summary>
        /// The downloader should advance to the next item.
        /// </summary>
        Item,
        /// <summary>
        /// The downloader should advance to the next collection.
        /// </summary>
        Collection,
        /// <summary>
        /// The downloader should stop and return immediately.
        /// </summary>
        Fail
    }
}