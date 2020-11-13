using Athame.Plugin.Api.Interface;

namespace Athame.Core.Plugin
{
    public static class ServiceCastExtensions
    {
        public static IAuthenticatable AsAuthenticatable(this IMediaService service)
            => service as IAuthenticatable;

        public static IAuthenticatableAsync AsAuthenticatableAsync(this IMediaService service)
            => service as IAuthenticatableAsync;

        public static IUsernamePasswordAuthenticationAsync AsUsernamePasswordAuthenticatable(this IMediaService service)
            => service as IUsernamePasswordAuthenticationAsync;
    }
}
