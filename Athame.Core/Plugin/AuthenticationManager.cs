using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athame.PluginAPI.Service;

// ReSharper disable SuspiciousTypeConversion.Global

namespace Athame.Core.Plugin
{
    public class AuthenticationManager
    {
        private readonly HashSet<MusicService> authenticatingServices = new HashSet<MusicService>();

        private void EnsureNotAuthenticating(MusicService service)
        {
            if (IsAuthenticating(service))
            {
                throw new InvalidOperationException("Service is busy authenticating");
            }
        }

        private void ServiceBeginAuthenticating(MusicService service)
        {
            EnsureNotAuthenticating(service);
            lock (authenticatingServices)
            {
                authenticatingServices.Add(service);
            }
        }

        private void ServiceEndAuthenticating(MusicService service)
        {
            lock (authenticatingServices)
            {
                authenticatingServices.Remove(service);
            }
        }

        public bool CanAuthenticate(MusicService service)
        {
            return service is IAuthenticatable && !IsAuthenticating(service);
        }

        public bool CanRestore(MusicService service)
        {
            var ias = service.AsAuthenticatable();
            if (ias == null) return false;
            return ias.HasSavedSession && !ias.IsAuthenticated && !IsAuthenticating(service);
        }

        public bool IsAuthenticating(MusicService service)
        {
            lock (authenticatingServices)
            {
                return authenticatingServices.Contains(service);
            }
        }

        public async Task<AuthenticationResult> Authenticate(MusicService service)
        {
            ServiceBeginAuthenticating(service);
            try
            {
                return new AuthenticationResult(service, await service.AsAuthenticatableAsync().AuthenticateAsync());
            }
            finally
            {
                ServiceEndAuthenticating(service);
            }
        }

        public async Task<AuthenticationResult> Authenticate(MusicService service, string username, string password, bool rememberMe)
        {
            ServiceBeginAuthenticating(service);
            try
            {
                return
                    new AuthenticationResult(service, await service.AsUsernamePasswordAuthenticatable().AuthenticateAsync(username, password, rememberMe));
            }
            finally
            {
                ServiceEndAuthenticating(service);
            }
        }

        public async Task<AuthenticationResult> Restore(MusicService service)
        {
            ServiceBeginAuthenticating(service);
            try
            {
                return new AuthenticationResult(service, await service.AsAuthenticatable().RestoreAsync());
            }
            finally
            {
                ServiceEndAuthenticating(service);
            }
        }

        



    }
}
