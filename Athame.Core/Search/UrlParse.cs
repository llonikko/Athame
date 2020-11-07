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

        public MediaDescriptor Result { get; }

        public string GetMessage()
            => "";
    }

    public class UrlParseNoService : IUrlParseResult
    {
        public UrlParseStatus Status
            => UrlParseStatus.NoServiceFound;

        public MediaDescriptor Result { get; }

        public string GetMessage()
            => "Can't download this URL.";
    }

    public class UrlParseInvalid : IUrlParseResult
    {
        public UrlParseStatus Status
            => UrlParseStatus.InvalidUrl;

        public MediaDescriptor Result { get; }

        public string GetMessage()
            => "Invalid URL. Check that the URL begins with http:// or https://.";
    }

    public class UrlParseNoMedia : IUrlParseResult
    {
        public UrlParseStatus Status
            => UrlParseStatus.NoMedia;

        public MediaDescriptor Result { get; }

        public string GetMessage()
            => "The URL does not point to a valid track, album, artist or playlist.";
    }

    public class UrlParseSuccess : IUrlParseResult
    {
        public UrlParseStatus Status
            => UrlParseStatus.Success;

        public MediaDescriptor Result { get; }

        public string GetMessage()
            => $"{Result.MediaType} from {Result.MediaServiceName}";

        public UrlParseSuccess(MediaDescriptor descriptor)
        {
            Result = descriptor;
        }
    }
}
