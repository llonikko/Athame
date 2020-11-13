using Athame.Avalonia.ViewModels;
using Athame.Avalonia.Views;
using Athame.Core;
using Athame.Core.Plugin;
using Athame.Plugin.Api.Interface;
using Athame.Plugin.Api.Service;
using Splat;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Athame.Avalonia.Models
{
    public class ServiceAuthenticator
    {
        private readonly AuthenticationManager authManager;

        public ServiceAuthenticator()
            => authManager = Locator.Current.GetService<AthameApp>().AuthenticationManager;

        public ServiceRestoreWindow Restore(IMediaService service)
        {
            if (CanRestore(service))
            {
                return new ServiceRestoreWindow
                {
                    DataContext = new ServiceRestoreWindowViewModel(new List<IMediaService> { service })
                };
            }
            return ServiceRestoreWindow.Null;
        }

        public async Task<bool> AuthenticateInternal(IMediaService service)
        {
            var result = await authManager.Authenticate(service);
            if (result.IsAuthenticated)
            {
                return true;
            }

            if (result.Exception != null)
            {
                // Log.WriteException(Level.Error, Tag, result.Exception, "AM custom auth");
            }
            else
            {
            }
            return false;
        }

        private bool CanRestore(IMediaService service)
        {
            if (!authManager.CanRestore(service))
            {
                return false;
            }
            return true;
        }

        private bool CanAuthenticate(IMediaService service)
        {
            if (!authManager.CanAuthenticate(service))
            {
                return false;
            }
            return true;
        }
    }
}
