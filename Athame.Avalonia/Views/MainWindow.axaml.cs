using Athame.Avalonia.ViewModels;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Athame.Avalonia.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public RoutedViewHost MainViewHost 
            => this.FindControl<RoutedViewHost>("MainViewHost");

        public MenuItem ViewSettingsMenuItem 
            => this.FindControl<MenuItem>("ViewSettingsMenuItem");
        public MenuItem ViewHelpMenuItem 
            => this.FindControl<MenuItem>("ViewHelpMenuItem");
        public MenuItem ViewAboutAppMenuItem 
            => this.FindControl<MenuItem>("ViewAboutAppMenuItem");

        public MediaSearchView SearchView 
            => this.FindControl<MediaSearchView>("MediaSearchView");
        public ProgressStatusView ProgressStatusView 
            => this.FindControl<ProgressStatusView>("ProgressStatusView");
        public MediaItemsView MediaItemsView 
            => this.FindControl<MediaItemsView>("MediaItemsView");

        public Button DownloadButton 
            => this.FindControl<Button>("DownloadButton");
        public Button CancelButton 
            => this.FindControl<Button>("CancelButton");

        public MainWindow()
        {
            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.Router, v => v.MainViewHost.Router)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.ViewSettingsCommand, v => v.ViewSettingsMenuItem)
                    .DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.ViewAboutAppCommand, v => v.ViewAboutAppMenuItem)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.MediaSearch, v => v.SearchView.DataContext)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.ProgressStatus, v => v.ProgressStatusView.DataContext)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.MediaItems, v => v.MediaItemsView.DataContext)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.DownloadMediaCommand, v => v.DownloadButton)
                    .DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.CancelDownloadCommand, v => v.CancelButton)
                    .DisposeWith(disposables);

                this.WhenAnyObservable(x => x.ViewModel.CanRestoreCommand)
                    .Where(window => window != ServiceRestoreWindow.Null)
                    .Subscribe(async window => await window.ShowDialog(this))
                    .DisposeWith(disposables);
            });

            Observable
                .FromEventPattern(this, nameof(Opened))
                .Select(e => Unit.Default)
                .InvokeCommand(this, x => x.ViewModel.CanRestoreCommand);

            Observable
                .FromEventPattern(this, nameof(Closed))
                .Select(e => Unit.Default)
                .InvokeCommand(this, x => x.ViewModel.SaveSettingsCommand);

            InitializeComponent();
        }

        private void InitializeComponent()
            => AvaloniaXamlLoader.Load(this);
    }
}
