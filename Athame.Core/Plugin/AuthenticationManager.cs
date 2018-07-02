using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athame.Core.Logging;
using Athame.PluginAPI.Service;

// ReSharper disable SuspiciousTypeConversion.Global

namespace Athame.Core.Plugin
{
    public class AuthenticationManager
    {
        private const string Tag = nameof(AuthenticationManager);

        private readonly object lockObject = new object();
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
            lock (lockObject)
            {
                EnsureNotAuthenticating(service);
                var r = authenticatingServices.Add(service);
                Log.Debug(Tag, $"ServiceBeginAuthenticating, {service.Info.Name}, {r}, {authenticatingServices.Count}");
            }
        }

        private void ServiceEndAuthenticating(MusicService service)
        {
            lock (lockObject)
            {
                var r = authenticatingServices.Remove(service);
                Log.Debug(Tag, $"ServiceEndAuthenticating, {service.Info.Name}, {r}, {authenticatingServices.Count}");
            }
        }

        public bool CanAuthenticate(MusicService service)
        {
            return service is IAuthenticatable && !IsAuthenticating(service);
        }

        public bool NeedsAuthentication(MusicService service)
        {
            if (!CanAuthenticate(service)) return false;
            var ias = service.AsAuthenticatable();
            return !ias.HasSavedSession;
        }

        public bool CanRestore(MusicService service)
        {
            if (IsAuthenticating(service)) return false;
            var ias = service.AsAuthenticatable();
            if (ias == null) return false;
            return ias.HasSavedSession && !ias.IsAuthenticated && ias.Account != null;
        }

        public bool IsAuthenticating(MusicService service)
        {
            lock (lockObject)
            {
                var r = authenticatingServices.Contains(service);
                Log.Debug(Tag, $"IsAuthenticating, {service.Info.Name}, {r}, {authenticatingServices.Count}");
                return r;
            }
        }

        public async Task<AuthenticationResult> Authenticate(MusicService service)
        {
            ServiceBeginAuthenticating(service);
            try
            {
                return new AuthenticationResult(service, await service.AsAuthenticatableAsync().AuthenticateAsync(), null);
            }
            catch (Exception ex)
            {
                return new AuthenticationResult(service, false, ex);
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
                    new AuthenticationResult(service,
                    await service.AsUsernamePasswordAuthenticatable().AuthenticateAsync(username, password, rememberMe), null);
            }
            catch (Exception ex)
            {
                return new AuthenticationResult(service, false, ex);
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
                return new AuthenticationResult(service, await service.AsAuthenticatable().RestoreAsync(), null);
            }
            catch (Exception ex)
            {
                return new AuthenticationResult(service, false, ex);
            }
            finally
            {
                ServiceEndAuthenticating(service);
            }
        }





    }
}
