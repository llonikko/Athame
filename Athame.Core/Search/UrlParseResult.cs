using Athame.Plugin.Api.Service;

namespace Athame.Core.Search
{
    /// <summary>
    /// Represents a parse result parsed with <see cref="UrlResolver.Parse"/>.
    /// </summary>
    public class UrlParseResult
    {
        public UrlParseStatus ParseStatus { get; set; }
        public MediaDescriptor MediaDescriptor { get; set; }

        public UrlParseResult()
        {
        }

        public UrlParseResult(UrlParseStatus status)
            => ParseStatus = status;

        public string GetMessage()
            => ParseStatus switch
            {
                UrlParseStatus.InvalidUrl => "Invalid URL. Check that the URL begins with http:// or https://.",
                UrlParseStatus.NoServiceFound => "Can't download this URL.",
                UrlParseStatus.NoMedia => "The URL does not point to a valid track, album, artist or playlist.",
                UrlParseStatus.Success => $"{MediaDescriptor.MediaType} from {MediaDescriptor.MediaServiceName}",
                _ => string.Empty
            };
    }
}
