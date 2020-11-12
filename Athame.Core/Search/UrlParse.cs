using Athame.Core.Interface;
using Athame.Plugin.Api.Service;

namespace Athame.Core.Search
{
    public static class UrlParse
    {
        public static IUrlParseResult Empty
            => new UrlParseInvalid();
        public static IUrlParseResult NoService
            => new UrlParseNoService();
        public static IUrlParseResult NoMedia
            => new UrlParseNoMedia();
        public static IUrlParseResult Invalid
            => new UrlParseInvalid();
        public static IUrlParseResult Success(MediaDescriptor descriptor)
            => new UrlParseSuccess(descriptor);
    }

    public class UrlParseEmpty : IUrlParseResult
    {
        public UrlParseStatus Status
            => UrlParseStatus.NullOrEmptyString;

        public string Message
            => "";
    }

    public class UrlParseNoService : IUrlParseResult
    {
        public UrlParseStatus Status
            => UrlParseStatus.NoServiceFound;

        public string Message
            => "Can't download this URL.";
    }

    public class UrlParseInvalid : IUrlParseResult
    {
        public UrlParseStatus Status
            => UrlParseStatus.InvalidUrl;

        public string Message
            => "Invalid URL. Check that the URL begins with http:// or https://.";
    }

    public class UrlParseNoMedia : IUrlParseResult
    {
        public UrlParseStatus Status
            => UrlParseStatus.NoMedia;

        public string Message
            => "The URL does not point to a valid track, album, artist or playlist.";
    }

    public class UrlParseSuccess : IUrlParseResult
    {
        public UrlParseStatus Status
            => UrlParseStatus.Success;

        public MediaDescriptor MediaDescriptor { get; }

        public string Message
            => $"{MediaDescriptor.MediaType} from {MediaDescriptor.MediaServiceName}";

        public UrlParseSuccess(MediaDescriptor descriptor)
        {
            MediaDescriptor = descriptor;
        }
    }
}
