using Athame.Avalonia.Models;
using Athame.Core.Search;

namespace Athame.Avalonia.Extensions
{
    public static class UrlResolverExtensions
    {
        public static string GetMessage(this UrlResolver resolver, UrlParseResult result)
            => result switch
            {
                UrlParseResult.NullOrEmptyString
                    => string.Empty,
                UrlParseResult.InvalidUrl
                    => UrlValidationMessage.UrlInvalid,
                UrlParseResult.NoServiceFound
                    => UrlValidationMessage.UrlNoService,
                UrlParseResult.ServiceNotAuthenticated
                    => string.Format(UrlValidationMessage.UrlNeedsAuthentication, resolver.Service.Name),
                UrlParseResult.NoMedia
                    => UrlValidationMessage.UrlNotParseable,
                UrlParseResult.Success
                    => string.Format(UrlValidationMessage.UrlValidParseResult, resolver.MediaUri.MediaType, resolver.Service.Name),
                UrlParseResult.Exception
                    => UrlValidationMessage.UrlExceptionOccurred,
                UrlParseResult.ServiceNotRestored
                    => string.Format(UrlValidationMessage.UrlNeedsRestore, resolver.Service.Name),
                _ => string.Empty
            };
    }
}
