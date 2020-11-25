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

        public AuthenticationViewModel AuthenticationViewModel { get; }
        public PluginDetailsViewModel PluginDetailsViewModel { get; }

        public ReactiveCommand<Unit, Unit> LogoutCommand { get; }

        public PluginSettingsViewModel(IPlugin plugin)
        {
            this.plugin = plugin;

            AuthenticationViewModel = new AuthenticationViewModel(this.plugin.Service);
            PluginDetailsViewModel = new PluginDetailsViewModel(this.plugin);

            LogoutCommand = ReactiveCommand.Create(Logout);

            this.WhenAnyValue(x => x.AuthenticationViewModel.IsAuthenticated)
                .Where(authenticated => authenticated)
                .Select(_ => Unit.Default)
                .InvokeCommand(ReactiveCommand.Create(() =>
                {
                    AuthenticationViewModel.Clear();
                    Update();
                }));

            UpdateViews();
        }

        private void Logout()
        {
            plugin.Service.Reset();
            Update();
        }

        private void Update()
        {
            plugin.Settings.Save();
            UpdateViews();
        }

        private void UpdateViews()
        {
            var service = plugin.Service;

            if (service.IsAuthenticated)
            {
                IsAuthenticated = true;
                AuthenticationStatus = $"Logged in as {service.Account.DisplayName}";
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

            PluginServiceSettingsView = plugin.SettingsControl.GetSettingsControl() as UserControl;
        }
    }
}
