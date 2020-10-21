using Athame.Avalonia.Extensions;
using Athame.Avalonia.Views;
using Athame.Core;
using Athame.Core.Download;
using Athame.Core.Extensions;
using Avalonia.Controls;
using ReactiveUI;
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
        private readonly MediaDownloadSource source;

        public ReactiveCommand<Unit, Unit> DownloadMediaCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelDownloadCommand { get; }
        public ReactiveCommand<Unit, Window> CanRestoreCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveSettingsCommand { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> ViewSettingsCommand { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> ViewAboutAppCommand { get; }

        public SearchViewModel SearchViewModel { get; }
        public DownloadStatusViewModel DownloadStatusViewModel { get; }
        public MediaItemsViewModel MediaItemsViewModel { get; }
        public RoutingState Router { get; }
        public ViewModelActivator Activator { get; }

        public MainWindowViewModel()
        {
            app = Locator.Current.GetService<AthameApp>();
            downloader = new MediaDownloader();
            source = new MediaDownloadSource();

            Router = new RoutingState();
            SearchViewModel = new SearchViewModel();
            DownloadStatusViewModel = new DownloadStatusViewModel();
            MediaItemsViewModel = new MediaItemsViewModel(source);

            DownloadMediaCommand = ReactiveCommand.CreateFromTask(
                DownloadMedia,
                MediaItemsViewModel.CanDownload);
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
                this.WhenAnyValue(x => x.SearchViewModel.SearchResult)
                    .Where(media => media != null)
                    .Subscribe(AddMedia)
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
            DownloadStatusViewModel.MediaDownloadStatus = $"{current.Index + 1}/{current.Total}: "
                + $"{current.Service.Media.MediaType} - {current.Service.Media.Title}";
        }

        private void TrackDownloadProgressed(TrackDownloadEventArgs e)
        {
            DownloadStatusViewModel.TrackDownloadStatus = e.Status.GetDescription();
            DownloadStatusViewModel.TrackDownloadProgressPercentage = e.PercentCompleted;
            DownloadStatusViewModel.TrackDownloadTitle = $"{e.TrackFile.Track.Artist} - {e.TrackFile.Track.Title}";
        }

        private void TrackDownloadCompleted(TrackDownloadEventArgs e)
        {
            DownloadStatusViewModel.TrackDownloadStatus = e.Status.GetDescription();
            MediaItemsViewModel.UpdateTrackItem(e.TrackFile.Track);
        }

        public void TrackDownloadSkipped(TrackDownloadEventArgs e)
        {
        }

        private void AddMedia(MediaDownloadService media)
            => source.Add(media);

        private void RemoveMedia()
        {
        }

        private async Task DownloadMedia()
        {
            ApplySettings();

            DownloadStatusViewModel.MediaDownloadStatus = "Warming up...";
            await Task.Delay(2000);

            await downloader.DownloadMediaAsync(source.Items);

            DownloadStatusViewModel.MediaDownloadStatus = "All downloads completed";
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
