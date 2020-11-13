using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Athame.Plugin.Api.Interface;
using Serilog;

namespace Athame.Core.Plugin
{
    public class AuthenticationManager
    {
        private readonly object lockObject = new object();
        private readonly HashSet<IMediaService> authenticableServices = new HashSet<IMediaService>();

        public IEnumerable<IMediaService> CanRestore(IEnumerable<IMediaService> services)
            => services.Where(CanRestore);

        public IEnumerable<Task<AuthenticationResult>> Restore(IEnumerable<IMediaService> services)
            => services.Select(Restore);

        public Task<AuthenticationResult> Authenticate(IMediaService service, string username, string password, bool rememberUser)
            => Authenticate(service, service.AsUsernamePasswordAuthenticatable().SetCredentials(username, password, rememberUser).AuthenticateAsync);

        public Task<AuthenticationResult> Authenticate(IMediaService service)
            => Authenticate(service, service.AsAuthenticatableAsync().AuthenticateAsync);

        public Task<AuthenticationResult> Restore(IMediaService service)
            => Authenticate(service, service.AsAuthenticatable().RestoreAsync);

        private void ServiceBeginAuthenticating(IMediaService service)
        {
            lock (lockObject)
            {
                EnsureNotAuthenticating(service);
                var result = authenticableServices.Add(service);
                Log.Debug("ServiceBeginAuthenticating, {Service}, {Result}, {Count}", service.Name, result, authenticableServices.Count);
            }
        }

        private void ServiceEndAuthenticating(IMediaService service)
        {
            lock (lockObject)
            {
                var result = authenticableServices.Remove(service);
                Log.Debug("ServiceEndAuthenticating, {Service}, {Result}, {Count}", service.Name, result, authenticableServices.Count);
            }
        }

        public bool NeedsAuthentication(IMediaService service)
            => CanAuthenticate(service) && !service.AsAuthenticatable().HasSavedSession;

        public bool CanRestore(IMediaService service)
        {
            if (IsAuthenticating(service))
            {
                return false;
            }
            var ias = service.AsAuthenticatable();
            if (ias == null)
            {
                return false;
            }
            return ias.HasSavedSession && ias.Account != null;
        }

        private void EnsureNotAuthenticating(IMediaService service)
        {
            if (IsAuthenticating(service))
            {
                throw new InvalidOperationException("Service is busy authenticating");
            }
        }

        public bool CanAuthenticate(IMediaService service) 
            => (service is IAuthenticatable) && !IsAuthenticating(service);

        public bool IsAuthenticating(IMediaService service)
        {
            lock (lockObject)
            {
                var result = authenticableServices.Contains(service);
                Log.Debug("IsAuthenticating, {Service}, {Result}, {Count}", service.Name, result, authenticableServices.Count);
                return result;
            }
        }

        private async Task<AuthenticationResult> Authenticate(IMediaService service, Func<Task<bool>> action)
        {
            bool isAuthenticated = false;
            Exception exception = null;

            ServiceBeginAuthenticating(service);
            try
            {
                isAuthenticated = await action.Invoke().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            ServiceEndAuthenticating(service);

            return new AuthenticationResult
            {
                ServiceName = service.Name,
                IsAuthenticated = isAuthenticated,
                Exception = exception
            };
        }
    }
}
