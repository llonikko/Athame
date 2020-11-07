using Athame.Avalonia.Models;
using Athame.Avalonia.Resources;
using Athame.Plugin.Api.Service;
using DynamicData;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

namespace Athame.Avalonia.ViewModels
{
    public class MediaItemsViewModel : ViewModelBase
    {
        private readonly ReadOnlyObservableCollection<MediaViewItem> items;

        public IObservable<bool> CanDownload { get; }
        public ReadOnlyObservableCollection<MediaViewItem> MediaItems 
            => items;

        public MediaItemsViewModel(MediaDownloadSource source)
        {
            source
                .Connect()
                .Transform(ms => new MediaViewItem(ms.Media))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out items)
                .DisposeMany()
                .Subscribe();

            CanDownload = this
                .WhenAnyValue(x => x.MediaItems.Count)
                .Select(count => count > 0 && items.Any(x => x.IsDownloadable()));
        }

        public void UpdateTrackViewItem(Track track, bool success)
        {
            var id = track.Playlist?.Id ?? track.Album.Id;
            var tv = items
                .SingleOrDefault(m => m.Id == id)
                .GetTrackViewItem(track);
            tv.ImageStatus = success ? Images.Success : Images.Error;
        }
    }
}
