using Athame.Avalonia.Models;
using Athame.Core.Download;
using DynamicData;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace Athame.Avalonia.ViewModels
{
    public class MediaItemsViewModel : ViewModelBase
    {
        public readonly MediaDownloadSource Source;

        private readonly ReadOnlyObservableCollection<MediaItem> mediaItems;
        public ReadOnlyObservableCollection<MediaItem> MediaItems => mediaItems;

        public IObservable<bool> IsNotEmpty { get; }

        public ReactiveCommand<MediaDownloadService, Unit> AddMediaCommand { get; }

        public MediaItemsViewModel()
        {
            Source = new MediaDownloadSource();
            
            Source
                .Connect()
                .Transform(ms => new MediaItem(ms))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out mediaItems)
                .DisposeMany()
                .Subscribe();

            IsNotEmpty = this
                .WhenAnyValue(x => x.MediaItems.Count)
                .Select(count => count > 0 && IsDownloadable(mediaItems));

            AddMediaCommand = ReactiveCommand.Create<MediaDownloadService>(AddMedia);
        }

        private void AddMedia(MediaDownloadService media)
            => Source.Add(media);

        private bool IsDownloadable(IEnumerable<MediaItem> items)
            => items.Any(item => item.TrackItems.Any(x => x.Track.IsDownloadable));
    }
}
