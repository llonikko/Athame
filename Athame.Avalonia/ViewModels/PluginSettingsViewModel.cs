using Athame.Avalonia.Models;
using Athame.Core;
using Athame.Core.Extensions;
using Athame.Core.Plugin;
using Athame.Plugin.Api;
using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;
using System.Reactive.Linq;

namespace Athame.Avalonia.ViewModels
{
    public class PluginSettingsViewModel : ViewModelBase
    {
        private readonly IPlugin plugin;

        [Reactive]
        public string AuthenticationStatus { get; set; }
        [Reactive]
        public bool IsAuthenticated { get; set; }
        [Reactive]
        public UserControl PluginServiceSettingsView { get; set; }

        public AuthenticationViewModel AuthenticationView { get; }
        public PluginDetailsViewModel PluginDetailsView { get; }

        public ReactiveCommand<Unit, Unit> LogoutCommand { get; }

        public PluginSettingsViewModel(IPlugin plugin)
        {
            this.plugin = plugin;

            AuthenticationView = new AuthenticationViewModel(this.plugin.Service);

            PluginDetailsView = new PluginDetailsViewModel(this.plugin);

            LogoutCommand = ReactiveCommand.Create(Logout);

            this.WhenAnyValue(x => x.AuthenticationView.IsAuthenticated)
                .Where(authenticated => authenticated)
                .Select(_ => Unit.Default)
                .InvokeCommand(ReactiveCommand.Create(() =>
                {
                    AuthenticationView.Clear();
                    Update();
                }));

            UpdateViews();
        }

        private void Logout()
        {
            plugin.Service.AsAuthenticatable().Reset();
            Update();
        }

        private void Update()
        {
            plugin.Settings.Save();
            UpdateViews();
        }

        private void UpdateViews()
        {
            var service = plugin.Service.AsAuthenticatable();

            if (service.IsAuthenticated)
            {
                IsAuthenticated = true;
                AuthenticationStatus = $"Logged in as {service.Account.FormattedName}";
            }
            else if (service.HasSavedSession)
            {
                IsAuthenticated = true;
                AuthenticationStatus = "You have saved credentials, but there was an error trying to restore your session.";
            }
            else
            {
                IsAuthenticated = false;
            }

            PluginServiceSettingsView = plugin.SettingsControl as UserControl;
        }

        //private Window Restore()
        //{
        //    var window = new ServiceAuthenticator().Restore(plugin.PluginService);
        //    if (window != null)
        //    {
        //        window.Closed += (s, e) =>
        //        {
        //            UpdateViews(plugin.PluginService.AsAuthenticatable());
        //            plugin.Settings.Save();
        //        };
        //    }
        //    return window;
        //}

        //private Window LogIn()
        //{
        //    var auth = Locator.Current.GetService<AthameApp>().AuthenticationManager;
        //    var authenticatableService = plugin.PluginService.AsAuthenticatable();

        //    if (auth.NeedsAuthentication(plugin.PluginService))
        //    {
        //        var window = new ServiceAuthenticator().Authenticate(plugin.PluginService);
        //        if (window != null)
        //        {
        //            window.Closed += (s, e) =>
        //            {
        //                UpdateViews(plugin.PluginService.AsAuthenticatable());
        //                plugin.Settings.Save();
        //            };
        //        }
        //        return window;
        //    }

        //    LogOut(authenticatableService);
        //    return null;
        //}
    }
}
