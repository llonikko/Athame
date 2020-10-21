using Athame.Avalonia.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Athame.Avalonia.Views
{
    public class PluginSettingsView : ReactiveUserControl<PluginSettingsViewModel>
    {
        public Panel AuthenticationStatusPanel => this.FindControl<Panel>("AuthenticationStatusPanel");
        public Panel AuthenticationPanel => this.FindControl<Panel>("AuthenticationPanel");

        public AuthenticationView AuthenticationView => this.FindControl<AuthenticationView>("AuthenticationView");
        public ContentControl PluginServiceSettingsViewContentControl => this.FindControl<ContentControl>("PluginServiceSettingsViewContentControl");
        public PluginDetailsView PluginDetailsView => this.FindControl<PluginDetailsView>("PluginDetailsView");

        public TextBlock AuthenticationStatusTextBlock => this.FindControl<TextBlock>("AuthenticationStatusTextBlock");
        public Button LogoutButton => this.FindControl<Button>("LogoutButton");

        public PluginSettingsView()
        {
            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.IsAuthenticated, v => v.AuthenticationStatusPanel.IsVisible, isAuthenticated => isAuthenticated)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.IsAuthenticated, v => v.AuthenticationPanel.IsVisible, isAuthenticated => !isAuthenticated)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.AuthenticationViewModel, v => v.AuthenticationView.DataContext)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.PluginServiceSettingsView, v => v.PluginServiceSettingsViewContentControl.Content)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.PluginDetailsViewModel, v => v.PluginDetailsView.DataContext)
                    .DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.AuthenticationStatus, v => v.AuthenticationStatusTextBlock.Text)
                    .DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.LogoutCommand, v => v.LogoutButton)
                    .DisposeWith(disposables);
            });

            InitializeComponent();
        }

        private void InitializeComponent()
            => AvaloniaXamlLoader.Load(this);
    }
}