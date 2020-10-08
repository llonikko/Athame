using Athame.Avalonia.Extensions;
using Athame.Avalonia.Views;
using Athame.Core;
using Athame.Core.Download;
using Athame.Core.Extensions;
using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Serilog;
using Splat;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Athame.Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IScreen, IActivatableViewModel
    {
        private readonly AthameApp app;
        private readonly MediaDownloader downloader;

        [Reactive]
        public string DownloadStatus { get; set; }
        [Reactive]
        public string TrackDownloadStatus { get; set; }
        [Reactive]
        public int TrackDownloadProgress { get; set; }
        [Reactive]
        public string TrackDownloadProgressText { get; set; }

        public ReactiveCommand<Unit, Unit> DownloadMediaCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelDownloadCommand { get; }

        public ReactiveCommand<Unit, Window> CanRestoreCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveSettingsCommand { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> ViewSettingsCommand { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> ViewAboutAppCommand { get; }

        public SearchViewModel SearchView { get; }
        public MediaItemsViewModel MediaItemsView { get; }
        public RoutingState Router { get; }
        public ViewModelActivator Activator { get; }

        public MainWindowViewModel()
        {
            app = Locator.Current.GetService<AthameApp>();

            Router = new RoutingState();

            SearchView = new SearchViewModel();
           
            MediaItemsView = new MediaItemsViewModel();

            downloader = new MediaDownloader();

            DownloadMediaCommand = ReactiveCommand.CreateFromTask(
                DownloadMedia,
                MediaItemsView.IsNotEmpty);

            CancelDownloadCommand = ReactiveCommand.Create(RemoveMedia);

            CanRestoreCommand = ReactiveCommand.Create(CanRestore);

            SaveSettingsCommand = ReactiveCommand.Create(SaveSettings);

            ViewSettingsCommand = ReactiveCommand.CreateFromObservable(
                Router.Navigate<SettingsViewModel>);

            ViewAboutAppCommand = ReactiveCommand.CreateFromObservable(
                Router.Navigate<AboutMeViewModel>);

            Activator = new ViewModelActivator();
            this.WhenActivated((CompositeDisposable disposables) =>
            {
                this.WhenAnyValue(x => x.SearchView.SearchResult)
                    .Where(media => media != null)
                    .InvokeCommand(MediaItemsView.AddMediaCommand)
                    .DisposeWith(disposables);

                Observable
                    .FromEventPattern<MediaDownloadEventArgs>(downloader, nameof(downloader.MediaDownloadStarted))
                    .Subscribe(e => MediaServiceDequeued(e.EventArgs))
                    .DisposeWith(disposables);
                Observable
                    .FromEventPattern<TrackDownloadEventArgs>(downloader, nameof(downloader.TrackDownloadStarted))
                    .Subscribe()
                    .DisposeWith(disposables);
                Observable
                    .FromEventPattern<TrackDownloadEventArgs>(downloader, nameof(downloader.TrackDownloadProgressed))
                    .Subscribe(e => TrackDownloadProgressed(e.EventArgs))
                    .DisposeWith(disposables);
                Observable
                    .FromEventPattern<TrackDownloadEventArgs>(downloader, nameof(downloader.TrackDownloadCompleted))
                    .Subscribe(e => TrackDownloadCompleted(e.EventArgs))
                    .DisposeWith(disposables);
                Observable
                    .FromEventPattern<TrackDownloadEventArgs>(downloader, nameof(downloader.TrackDownloadSkipped))
                    .Subscribe(e => TrackDownloadSkipped(e.EventArgs))
                    .DisposeWith(disposables);
            });
        }

        private void MediaServiceDequeued(MediaDownloadEventArgs e)
        {
            var current = e;
            DownloadStatus = $"{current.Index + 1}/{current.Total}: "
                + $"{current.Service.Media.MediaType} - {current.Service.Media.Title}";
        }

        private void TrackDownloadProgressed(TrackDownloadEventArgs e)
        {
            TrackDownloadStatus = e.Status.GetDescription();
            TrackDownloadProgress = e.PercentCompleted;
            TrackDownloadProgressText = $"{TrackDownloadProgress}%";
        }

        private void TrackDownloadCompleted(TrackDownloadEventArgs e)
        {
            TrackDownloadStatus = e.Status.GetDescription();
        }

        public void TrackDownloadSkipped(TrackDownloadEventArgs e)
        {
        }

        private void RemoveMedia()
        {
        }

        private async Task DownloadMedia()
        {
            ApplySettings();

            DownloadStatus = "Warming up...";
            await Task.Delay(2000);

            await downloader.DownloadMediaAsync(MediaItemsView.Source.Items);

            DownloadStatus = "All downloads completed";
        }

        private Window CanRestore()
        {
            app.LoadAndInitPlugins();

            var services = app.AuthenticationManager.CanRestore(app.PluginServices);

            return services.Any()
                ? new ServiceRestoreWindow { DataContext = new ServiceRestoreWindowViewModel(services) }
                : ServiceRestoreWindow.Null;
        }

        private void SaveSettings()
        {
            Log.Debug("Save Settings");
            app.AppSettings.Save();
            app.Plugins.ForEach(p => p.Settings.Save());
        }

        private void ApplySettings()
        {
            Log.Debug("Applying current settings");
            downloader.ApplySettings(app.AppSettings.Current);
        }
    }
}
