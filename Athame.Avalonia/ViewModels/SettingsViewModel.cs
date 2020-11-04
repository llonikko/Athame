using Athame.Avalonia.Models;
using Athame.Avalonia.Views;
using Athame.Core;
using Athame.Core.Settings;
using Athame.Core.Utilities;
using Athame.Plugin.Api.Service;
using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Athame.Avalonia.ViewModels
{
    public class SettingsViewModel : ViewModelBase, IRoutableViewModel
    {
        private readonly AthameSettings settings;

        #region Commands
        public ReactiveCommand<Unit, Unit> SaveCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelCommand { get; }
        public ReactiveCommand<Unit, Unit> SelectAlbumLocationCommand { get; }
        public ReactiveCommand<Unit, Unit> SelectPlaylistLocationCommand { get; }
        public ReactiveCommand<Unit, Window> ViewPathFormatHelpCommand { get; }
        #endregion

        #region App Settings Properties
        public string AlbumLocation
        {
            get => settings.GeneralPreference.Location;
            set
            {
                settings.GeneralPreference.Location = value;
                this.RaisePropertyChanged();
            }
        }
        public string PlaylistLocation
        {
            get => settings.PlaylistPreference.Location;
            set
            {
                settings.PlaylistPreference.Location = value;
                this.RaisePropertyChanged();
            }
        }
        public string AlbumPathFormat
        {
            get => settings.GeneralPreference.PathFormat;
            set
            {
                settings.GeneralPreference.PathFormat = value;
                this.RaisePropertyChanged();
            }
        }
        public string PlaylistPathFormat
        {
            get => settings.PlaylistPreference.PathFormat;
            set
            {
                settings.PlaylistPreference.PathFormat = value;
                this.RaisePropertyChanged();
            }
        }
        public bool DontSavePlaylistArtwork
        {
            get => settings.DontSavePlaylistArtwork;
            set => settings.DontSavePlaylistArtwork = value;
        }
        public int PlaylistFileType
        {
            get => (int)settings.PlaylistFileType;
            set => settings.PlaylistFileType = (PlaylistFileType)value;
        }
        public bool AskBeforeExit
        {
            get => settings.ConfirmExit;
            set => settings.ConfirmExit = value;
        }
        public bool WriteWatermarkTag
        {
            get => settings.WriteWatermark;
            set => settings.WriteWatermark = value;
        }
        #endregion

        [Reactive]
        public string SampleAlbumPath { get; set; }
        [Reactive]
        public string SamplePlaylistPath { get; set; }
        [Reactive]
        public bool IsAlbumPathValid { get; set; }
        [Reactive]
        public bool IsPlaylistPathValid { get; set; }

        [Reactive]
        public int SelectedPlugin { get; set; }
        public PluginSettingsViewModel PluginSettingsView { [ObservableAsProperty]get; }
        public IEnumerable PluginServices { get; }

        public string UrlPathSegment => "Settings";
        public IScreen HostScreen { get; }

        public SettingsViewModel()
        {
            var app = Locator.Current.GetService<AthameApp>();
            settings = app.AppSettings.Clone() as AthameSettings;

            HostScreen = Locator.Current.GetService<IScreen>();
            PluginServices = app.Plugins.Select(p => p.Name);

            this.WhenAnyValue(x => x.AlbumPathFormat)
                .Select(format => format.Trim())
                .Subscribe(format =>
                {
                    IsAlbumPathValid = PathHelpers.TryFormat(format, MediaSample.Album, out string result);
                    SampleAlbumPath = result;
                });

            this.WhenAnyValue(x => x.PlaylistPathFormat)
                .Select(format => format.Trim())
                .Subscribe(format =>
                {
                    IsPlaylistPathValid = PathHelpers.TryFormat(format, MediaSample.Playlist, out string result);
                    SamplePlaylistPath = result;
                });

            this.WhenAnyValue(x => x.SelectedPlugin)
                .Select(p => new PluginSettingsViewModel(app.Plugins[p]))
                .ToPropertyEx(this, x => x.PluginSettingsView);

            var canSave = this
                .WhenAnyValue(
                    x => x.IsAlbumPathValid,
                    x => x.IsPlaylistPathValid,
                    (albumValid, playlistValid) =>
                        albumValid && playlistValid);
            SaveCommand = ReactiveCommand.CreateFromObservable(() =>
            {
                app.AppSettings = settings;
                app.AppSettings.Save();
                app.Plugins[SelectedPlugin]?.Settings.Save();
                return NavigateBack();
            },
                canSave);

            CancelCommand = ReactiveCommand.CreateFromObservable(NavigateBack);
            SelectAlbumLocationCommand = ReactiveCommand.CreateFromTask(SelectAlbumDirectory);
            SelectPlaylistLocationCommand = ReactiveCommand.CreateFromTask(SelectPlaylistDirectory);
            ViewPathFormatHelpCommand = ReactiveCommand.Create(GetPathFormatHelpWindow);
        }

        private async Task SelectAlbumDirectory()
        {
            var folderBrowser = Locator.Current.GetService<FolderBrowserDialog>();
            AlbumLocation = await folderBrowser.SelectFolder(AlbumLocation);
        }

        private async Task SelectPlaylistDirectory()
        {
            var folderBrowser = Locator.Current.GetService<FolderBrowserDialog>();
            PlaylistLocation = await folderBrowser.SelectFolder(PlaylistLocation);
        }

        private IObservable<Unit> NavigateBack()
            => HostScreen.Router.NavigateBack.Execute();

        private Window GetPathFormatHelpWindow() => new PathFormatHelpWindow();
        //=> Locator.Current.GetService<Window>("PathFormatHelp");
    }
}
