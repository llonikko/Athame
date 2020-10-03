namespace Athame.Avalonia.Models
{
    public static class UrlValidationMessage
    {
        public static readonly string UrlInvalid = "Invalid URL. Check that the URL begins with http:// or https://.";
        public static readonly string UrlNoService = "Can't download this URL.";
        public static readonly string UrlNeedsAuthentication = "You need to sign in to {0} first. " + UrlNeedsAuthenticationLink1;
        public static readonly string UrlNeedsAuthenticationLink1 = "Click here to sign in.";
        public static readonly string UrlNeedsRestore = "Couldn't sign in to {0}. Go to Settings to attempt sign-in again.";
        public static readonly string UrlNotParseable = "The URL does not point to a valid track, album, artist or playlist.";
        public static readonly string UrlValidParseResult = "{0} from {1}";
        public static readonly string UrlExceptionOccurred = "An exception occurred while trying to parse the URL.";
    }
}
