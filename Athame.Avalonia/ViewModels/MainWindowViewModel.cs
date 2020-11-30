using Athame.Avalonia.Extensions;
using Athame.Core;
using Athame.Core.Download;
using Athame.Core.Extensions;
using Athame.Plugin.Api.Downloader;
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
        private readonly MediaDownloadManager downloader;
        private readonly MediaDownloadSource source;

        public ReactiveCommand<Unit, Unit> DownloadMediaCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelDownloadCommand { get; }
        public ReactiveCommand<Unit, ServiceRestoreWindowViewModel> CanRestoreCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveSettingsCommand { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> ViewSettingsCommand { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> ViewAboutAppCommand { get; }

        public MediaSearchViewModel MediaSearch { get; }
        public ProgressStatusViewModel ProgressStatus { get; }
        public MediaItemsViewModel MediaItems { get; }

        public RoutingState Router { get; }
        public ViewModelActivator Activator { get; }

        public MainWindowViewModel()
        {
            app = Locator.Current.GetService<AthameApp>();
            downloader = new MediaDownloadManager();
            source = new MediaDownloadSource();

            Router = new RoutingState();

            MediaSearch = new MediaSearchViewModel();
            ProgressStatus = new ProgressStatusViewModel();
            MediaItems = new MediaItemsViewModel(source);

            DownloadMediaCommand = ReactiveCommand.CreateFromTask(
                DownloadMedia,
                MediaItems.CanDownload);
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
                this.WhenAnyValue(x => x.MediaSearch.SearchResult)
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
                    .FromEventPattern<TrackDownloadEventArgs>(
                        downloader.TrackDownloader, nameof(downloader.TrackDownloader.TrackDownloadStarted), RxApp.MainThreadScheduler)
                    .Subscribe(e => TrackDownloadStarted(e.EventArgs));
                Observable
                    .FromEventPattern<TrackDownloadEventArgs>(downloader, nameof(downloader.TrackDownloadProgressChanged))
                    .Subscribe(e => TrackDownloadProgressed(e.EventArgs))
                    .DisposeWith(disposables);
                Observable
                    .FromEventPattern<TrackDownloadEventArgs>(downloader, nameof(downloader.TrackDownloadCompleted))
                    .Subscribe(e => TrackDownloadCompleted(e.EventArgs))
                    .DisposeWith(disposables);
            });
        }

        private void MediaServiceDequeued(MediaDownloadEventArgs e)
        {
            var current = e;
            ProgressStatus.MediaDownloadStatus = $"{current.Index + 1}/{current.Total}: "
                + $"{current.CurrentMediaDownload.Media.MediaType} - {current.CurrentMediaDownload.Media.Title}";
        }

        private void TrackDownloadStarted(TrackDownloadEventArgs e)
        {
            ProgressStatus.TrackDownloadStatus = e.Status.GetDescription();
            ProgressStatus.TrackDownloadProgressPercentage = e.PercentCompleted;
            ProgressStatus.TrackDownloadTitle = $"{e.TrackFile.Track.Artist} - {e.TrackFile.Track.Title}";
        }

        private void TrackDownloadProgressed(TrackDownloadEventArgs e)
        {
            ProgressStatus.TrackDownloadStatus = e.Status.GetDescription();
            ProgressStatus.TrackDownloadProgressPercentage = e.PercentCompleted;
        }

        private void TrackDownloadCompleted(TrackDownloadEventArgs e)
        {
            ProgressStatus.TrackDownloadStatus = e.Status.GetDescription();

            if (TrackStatus.DownloadSkipped == e.Status)
            {
                MediaItems.UpdateTrackViewItem(e.TrackFile.Track, e.TrackFile.Track.IsDownloadable);
            }
            else
            {
                MediaItems.UpdateTrackViewItem(e.TrackFile.Track, TrackStatus.DownloadCompleted == e.Status);
            }
        }

        private void AddMedia(MediaItem media)
            => source.Add(media);

        private void RemoveMedia()
        {
        }

        private async Task DownloadMedia()
        {
            downloader.Settings = app.AppSettings;

            ProgressStatus.MediaDownloadStatus = "Warming up...";
            await Task.Delay(2000);

            await downloader.StartDownloadAsync(source.Items);

            ProgressStatus.MediaDownloadStatus = "All downloads completed";
        }

        private ServiceRestoreWindowViewModel CanRestore()
        {
            app.LoadPlugins();

            var services = app.AuthenticationManager.CanRestore(AthameApp.Services);

            return services.Any()
                ? new ServiceRestoreWindowViewModel(services)
                : ServiceRestoreWindowViewModel.Null;
        }

        private void SaveSettings()
        {
            Log.Debug("Save Settings");
            app.AppSettings.Save();
            app.Plugins.ForEach(p => p.Settings.Save());
        }
    }
}
