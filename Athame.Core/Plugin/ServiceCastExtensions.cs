using Athame.PluginAPI.Service;

// ReSharper disable SuspiciousTypeConversion.Global

namespace Athame.Core.Plugin
{
    public static class ServiceCastExtensions
    {

        public static IAuthenticatable AsAuthenticatable(this MusicService service)
        {
            return service as IAuthenticatable;
        }

        public static IAuthenticatableAsync AsAuthenticatableAsync(this MusicService service)
        {
            return service as IAuthenticatableAsync;
        }

        public static IUsernamePasswordAuthenticationAsync AsUsernamePasswordAuthenticatable(this MusicService service)
        {
            return service as IUsernamePasswordAuthenticationAsync;
        }
    }
}
